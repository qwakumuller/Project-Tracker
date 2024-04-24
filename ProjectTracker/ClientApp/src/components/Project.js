import React, { useCallback, useEffect, useState } from 'react';
import { useParams } from "react-router-dom";
import moment from "moment";
import { NotificationManager } from 'react-notifications'
import { Button } from 'primereact/button';
import { Dialog } from 'primereact/dialog';
import { Dropdown } from 'primereact/dropdown';
import "primereact/resources/themes/bootstrap4-light-blue/theme.css";
import "primereact/resources/primereact.min.css";
import "primeicons/primeicons.css";
import './Project.css';
import CreateTask from "./CreateTask";
import { SpeedDial } from 'primereact/speeddial';
import { TabView, TabPanel } from 'primereact/tabview';
import { Gantt } from 'gantt-task-react';
import "gantt-task-react/dist/index.css";
import UpdateProjectModal from "./UI/UpdateProjectModal";

export default function Project() {
    const [project, setProject] = useState({});
    const [addUserDialog, setAddUserDialog] = useState(false);
    const [allUserDialog, setAllUserDialog] = useState(false);
    const [statusDialog, setStatusDialog] = useState(false);
    const [tasks, setTasks] = useState([]);
    const [loading, setLoading] = useState(true);
    const [isOpen, setOpen] = useState(false);
    let { id } = useParams();
    const [selectedUser, setSelectedUser] = useState({});
    const [allUsers, setAllUsers] = useState([]);
    const [newUsers, setNewUsers] = useState([]);
    const [status, setStatus] = useState("");
    const [statuses, setStatuses] = useState([]);
    const [changeStatus, setChangeStatus] = useState(false);
    const [statusToChange, setStatusToChange] = useState(0);
    const [chartTasks, setChartTasks] = useState([]);
    const [show, setShow] = useState(false);
    const [editTask, setEditTask] = useState('');    
    const [edit, setEdit] = useState(false);
    const [activeIndex, setActiveIndex] = useState(0);

    const populateTasksData = useCallback(async () => {
        await fetch(`api/tasks/project/${id}`)
            .then(async (res) => {
                if (res.ok) {
                    const data = await res.json();
                    setTasks(data);
                    setLoading(false);
                } else {
                    const msg = await res.text();
                    console.log(msg);
                    setLoading(false);
                }
            });
    }, [id]);

    const populateChartTasksData = useCallback(async () => {
        await fetch(`api/tasks/project/${id}/chart-tasks`)
            .then(async (res) => {
                if (res.ok) {
                    const data = await res.json();
                    const temp = data.map(x => ({...x, start: new Date(x.start), end: new Date(x.end)}));
                    setChartTasks(temp);
                } else {
                    const msg = await res.text();
                    console.log(msg);
                }
            });
    }, [id]);

    const toggleModal = (event) => {
        event.preventDefault();
        setOpen(!isOpen);
        return false;
    };

    useEffect(() => {
        async function populateProjectData() {
            setLoading(true);
            await fetch(`api/project/${id}`)
                .then(async (res) => {
                    if (res.ok) {
                        const data = await res.json();
                        setProject(data);
                    } else {
                        const msg = await res.text();
                        console.log(msg);
                        setLoading(false);
                    }
                });
        };

        populateProjectData();
        populateTasksData();
        populateChartTasksData();

    }, [id, populateTasksData, populateChartTasksData]);

    async function getAllNewUsers() {
        await fetch(`api/project/${id}/new-users`)
            .then(async (res) => {
                if (res.ok) {
                    const data = await res.json();
                    setNewUsers(data);
                } else {
                    const msg = await res.text();
                    console.log(msg);
                }
            });
    };

    async function getAllUsers() {
        await fetch(`api/project/${id}/all-users`)
            .then(async (res) => {
                if (res.ok) {
                    const data = await res.json();
                    setAllUsers(data);
                } else {
                    const msg = await res.text();
                    console.log(msg);
                }
            });
    };

    async function addUser() {
        await fetch(`api/userproject`, {
            method: "POST",
            body: JSON.stringify({ ProjectId: project.projectId, UserId: selectedUser.userId }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
            }
        })
            .then(async (res) => {
                if (res.ok) {
                    await res.json();
                    NotificationManager.success("User was added successfully!");
                } else {
                    const msg = await res.text();
                    console.log(msg);
                    NotificationManager.error(msg);
                }
            });
    };

    async function deleteUser(userId) {
        await fetch(`api/userproject`, {
            method: "DELETE",
            body: JSON.stringify({ ProjectId: project.projectId, UserId: userId }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
            }
        })
            .then(async (res) => {
                if (res.ok) {
                    await res.json();
                    NotificationManager.success("User was deleted successfully!");
                    setAllUserDialog(false);
                } else {
                    const msg = await res.text();
                    console.log(msg);
                    NotificationManager.error(msg);
                }
            });
    };

    async function addStatus() {
        if (changeStatus === true) {
            await fetch(`api/status`, {
                method: "PUT",
                body: JSON.stringify({ StatusId: statusToChange, Name: status, ProjectId: project.projectId }),
                headers: {
                    'Content-type': 'application/json; charset=UTF-8',
                }
            })
                .then(async (res) => {
                    if (res.ok) {
                        NotificationManager.success("Status was updated successfully!");
                        setStatus("");
                    } else {
                        const msg = await res.text();
                        console.log(msg);
                        NotificationManager.error(msg);
                    }
                });

            setChangeStatus(false);
            setStatusToChange(0);
            setStatus("");
            populateTasksData();
            return;
        }
    
        await fetch(`api/status`, {
            method: "POST",
            body: JSON.stringify({ IsActive: true, Name: status, ProjectId: project.projectId }),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
            }
        })
            .then(async (res) => {
                if (res.ok) {
                    NotificationManager.success("Status was added successfully!");
                } else {
                    const msg = await res.text();
                    console.log(msg);
                    NotificationManager.error(msg);
                }
            });
        setStatus("");
    };

    async function deleteStatus(id) {
        await fetch(`api/status/${id}`, {
            method: "DELETE",
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
            }
        })
            .then(async (res) => {
                if (res.ok) {
                    NotificationManager.success("Status was deleted successfully!");
                    setStatusDialog(false);
                    setStatuses([]);
                    setStatus("");
                } else {
                    const msg = await res.text();
                    console.log(msg);
                    NotificationManager.error(msg);
                }
            });
    };

    async function getAllStatuses() {
        await fetch(`api/status/${project.projectId}/all`)
            .then(async (res) => {
                if (res.ok) {
                    const data = await res.json();
                    setStatuses(data);
                } else {
                    const msg = await res.text();
                    console.log(msg);
                }
            });
    };

    async function toggleStatus(id) {
        await fetch(`api/status/${id}/toggle`, {
            method: "PUT"
        })
            .then(async (res) => {
                if (res.ok) {
                    const message = await res.text();
                    NotificationManager.success(message);
                } else {
                    const msg = await res.text();
                    console.log(msg);
                }
            });
        setStatusDialog(false);
        setStatuses([]);
    };

    const handleChange = (e) => {
        setStatus(e.target.value);
    };

    const handleEdit = (task) => {
        setEdit(true);
        setEditTask(task);
        handleModal();
    };

    const handleAdd = () => {
        setEdit(false);
        handleModal();
    };

    const handleModal = () => {
        setShow(true);
    };

    const handleClose = () => {
        setShow(false)
        setEditTask('');
    }

    const items = (id, name) => [
        {
            label: 'Delete',
            icon: 'pi pi-trash',
            command: () => {
                deleteStatus(id);
            }
        },
        {
            label: 'Update',
            icon: 'pi pi-pencil',
            command: () => {
                setStatusToChange(id);
                setStatus(name);
                setChangeStatus(true);
            }
        },
        {
            label: 'Change',
            icon: 'pi pi-refresh',
            command: () => {
                toggleStatus(id);
            }
        }
    ];

    const handleTabChange = (i) => {
        setActiveIndex(i)
        if (i === 1)
            populateChartTasksData();
    };

    return (
        <div>
            <CreateTask populateTasksData={populateTasksData} handleClose={() => handleClose()} show={show} task={editTask} edit={edit} />
            {loading ? <p><em>Loading...</em></p> : ''}
            {project.name ?
                <div>
                    <h2>Project Name: <span className='text-secondary'>{project.name}</span></h2>
                    <h3>Project Description: <span className='text-secondary'>{project.description}</span></h3>
                    <h3>Start Date: <span className='text-secondary'>{moment(project.startDate).format('MM/DD/yyyy')}</span></h3>
                    <h3>End Date: <span className='text-secondary'>{moment(project.endDate).format('MM/DD/yyyy')}</span></h3>
                    <div className='mt-4'>
                        <div className='flex align-content-between'>
                            <Button size="sm" label="Add User" onClick={() => (((setAddUserDialog(true)), getAllNewUsers()))} />
                            <Button className='left-btn' size="sm" label="Show Users" onClick={() => (((setAllUserDialog(true)), getAllUsers()))} />
                            <Button className='left-btn' size="sm" label="Statuses" onClick={() => (((setStatusDialog(true)), getAllStatuses()))} />
                            <Button className='left-btn' size="sm" label="Add Task" onClick={() => (handleAdd())} />
                            <Button className='left-btn' size="sm" label="Update Project" onClick={toggleModal}/>
                            <UpdateProjectModal isOpen={isOpen} toggleModal={toggleModal} projId={id}/> 
                        </div>
                    </div>
                    <div className="card mt-3">
                        <TabView activeIndex={activeIndex} onTabChange={(e) => handleTabChange(e.index)}>
                            <TabPanel header="Tasks">
                                <table className='table table-striped mt-2'>
                                    <thead>
                                        <tr>
                                            <th>Name</th>
                                            <th>Description</th>
                                            <th>Start Date</th>
                                            <th>End Date</th>
                                            <th>Status</th>
                                            <th>Assigned User</th>
                                            <th>Task Status</th>
                                            <th>Prerequisite Task</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {tasks.map(t =>
                                            <tr key={t.taskId} id="taskRow" onClick={() => handleEdit(t)}>
                                                <td>{t.name}</td>
                                                <td>{t.description}</td>
                                                <td>{moment(t.startDate).format('MM/DD/yyyy')}</td>
                                                <td>{moment(t.endDate).format('MM/DD/yyyy')}</td>
                                                <td className={t.isComplete ? 'text-success' : 'text-danger'}>{t.isComplete ? "Completed" : "Incomplete"}</td>
                                                <td>{t.user ? `${t.user.firstName} ${t.user.lastName}` : `-`}</td>
                                                <td>{t.statusName}</td>
                                                <td className={t?.preTask?.isComplete ? 'text-success' : 'text-danger'} >{t.preTask != null ? `${t.preTask.taskName}` : "-"}</td>
                                            </tr>
                                        )}
                                    </tbody>
                                </table>
                            </TabPanel>
                            <TabPanel header="Chart">
                                <Gantt tasks={chartTasks} viewMode={"Month"} />
                            </TabPanel>
                        </TabView>
                    </div>
                    <Dialog header="Add User" visible={addUserDialog} style={{ width: '50vw' }} onHide={() => setAddUserDialog(false)}>
                        <Dropdown value={selectedUser} onChange={(e) => setSelectedUser(e.value)} options={newUsers} optionLabel="email"
                            placeholder="Select a User" className="w-full md:w-14rem" filter />
                        <div className='mt-5'>
                            <Button label="Close" icon="pi pi-times" onClick={() => setAddUserDialog(false)} className="p-button-warning mr_2" />
                            <Button label="Add" icon="pi pi-check" onClick={() => (((setAddUserDialog(false)), addUser()))} autoFocus />
                        </div>
                    </Dialog>
                    <Dialog header="All Users" visible={allUserDialog} style={{ width: '50vw' }} onHide={() => setAllUserDialog(false)}>
                        <table className='table table-striped'>
                            <thead>
                                <tr>
                                    <th>User Name</th>
                                    <th>User Email</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                {allUsers.map(r =>
                                    <tr key={r.userId}>
                                        <td>{`${r.firstName} ${r.lastName}`}</td>
                                        <td>{r.email}</td>
                                        <td><Button label="Delete" icon="pi pi-times" onClick={() => deleteUser(r.userId)} className="p-button-danger" /></td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                        <div className='mt-5'>
                            <Button label="Close" icon="pi pi-times" onClick={() => setAllUserDialog(false)} className="p-button-warning" />
                        </div>
                    </Dialog>
                    <Dialog header="Add Status" visible={statusDialog} style={{ width: '50vw' }} onHide={() => setStatusDialog(false)}>
                        <table className='table table-striped'>
                            <thead>
                                <tr>
                                    <th>Name</th>
                                    <th>Status</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                {statuses.map(s =>
                                    <tr key={s.statusId}>
                                        <td>{s.name}</td>
                                        <td>{s.isActive ? "Active" : "Inactive"}</td>
                                        <td><SpeedDial model={items(s.statusId, s.name)} direction="right" /></td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                        <div>
                            <div className="form-row mb-3">
                                <div className="form-group col-md-12">
                                    <label htmlFor="Name">Status Name:</label>
                                    <input id="Name" className="form-control col-sm-10" type="text" placeholder="Status Name" value={status} onChange={e => handleChange(e)} required />
                                </div>
                            </div>
                        </div>
                        <div className='mt-5'>
                            <Button label="Close" icon="pi pi-times" onClick={() => setStatusDialog(false)} className="p-button-warning mr_2" />
                            <Button label={changeStatus ? "Update" : "Add"} icon="pi pi-check" onClick={() => (((setStatusDialog(false)), addStatus()))} autoFocus />
                        </div>
                    </Dialog>
                </div> : <p>Invalid project ID</p>
            }
        </div>
    );
}