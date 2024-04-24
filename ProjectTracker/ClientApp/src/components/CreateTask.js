import { useState, useEffect, useCallback } from "react";
import Modal from 'react-bootstrap/Modal';
import Form from "react-bootstrap/Form";
import { NotificationManager } from "react-notifications";
import { useParams } from "react-router-dom";
import moment from "moment";

function CreateTask(props) {
    let { id } = useParams();

    const [task, setTask] = useState({
        TaskId: undefined, ProjectId: id, IsComplete: false, Name: "", Description: "", StartDate: "", EndDate: "", PrereqTaskId: undefined, UserId: undefined, StatusId: undefined
    });
    const [tasks, setTasks] = useState([]);
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [statuses, setStatuses] = useState([]);
    const [complete, setComplete] = useState(props.task.IsComplete);

    const handleShow = () => {    
        props.edit ?
            setTask(
            {
                TaskId: props.task.taskId.toString(), ProjectId: id, IsComplete: props.task.isComplete,
                Name: props.task.name, Description: props.task.description,
                StartDate: moment(props.task.startDate).format('yyyy-MM-DD'),
                EndDate: moment(props.task.endDate).format('yyyy-MM-DD'),
                UserId: (props.task.user ? props.task.user.id : undefined),
                PrereqTaskId: (props.task.prereqTaskId ? props.task.prereqTaskId : undefined),
                StatusId: (props.task.statusId ? props.task.statusId : undefined)
             })

            : setTask({
                ProjectId: id, IsComplete: false, Name: "", Description: "", StartDate: "",
                EndDate: "", PrereqTaskId: undefined, UserId: undefined, StatusId: undefined
             });
        setComplete(() => task.IsComplete);
        populateStatusData();
    }
    
    const handleExited = () => {props.populateTasksData(); populateTasksData()};

    const handleSubmit = (event) => {
        event.preventDefault();
        props.edit ? editTask() : createTask();
        props.handleClose();       
    }

    const handleChange = (event) => {
        const { name, value } = event.target;      
        setTask((prevState) => ({
            ...prevState, [name]: value,
        }))
    }


    const handleComplete = () => {
        setComplete(() => !complete);
        setTask((prevState) => ({
           ...prevState, IsComplete: complete,
        }))
    }  

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
   

    const populateStatusData = useCallback(async () => {
        await fetch(`api/status/${id}/only-active`)
            .then(async (res) => {
                if (res.ok) {
                    const data = await res.json();
                    console.log(data);
                    setStatuses(data);
                } else {
                    const msg = await res.text();
                    console.log(msg);
                }
            });
    }, [id]);

    const populateUserData = useCallback(async () => {
        setLoading(true);
        const response = await fetch(`api/project/${id}/all-users`);
        const data = await response.json();
        console.log(data);
        setUsers(data);
        setLoading(false);
    }, [id]);

    useEffect(() => {
        populateUserData();
        populateStatusData();
        populateTasksData();
    }, [populateStatusData, populateTasksData, populateUserData]);

    async function createTask() {
        await fetch('api/tasks', {
            method: "POST",
            body: JSON.stringify(task),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
            }
        })
            .then(async (response) => {
                if (response.ok) {
                    await response.json();
                    NotificationManager.success("Task Successfully Created");
                    await populateTasksData();
                } else {
                    const msg = await response.text();
                    console.log(msg);
                    NotificationManager.error("Task Creation UnSuccessful");
                }
            })
            .catch(err => {
                NotificationManager.error("Service Unavailable")
                console.log("error " + err);
            })

    }

    async function editTask() {
        await fetch('api/tasks', {
            method: "PUT",
            body: JSON.stringify(task),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
            }
        })
            .then(async (response) => {
                if (response.ok) {
                    await response.text();
                    NotificationManager.success("Task Successfully Updated");
                    await populateTasksData();
                } else {
                    const msg = await response.text();
                    console.log(msg);
                    NotificationManager.error("Task Update UnSuccessful");
                }
            })
            .catch(err => {
                NotificationManager.error("Service Unavailable")
                console.log("error " + err);
            })

    }

    return (
        <>          
            <Modal show={props.show} onEnter={handleShow} onHide={props.handleClose} onExited={handleExited} >
            <Modal.Header closeButton>
                    <Modal.Title>{props.edit ? 'Update Task' : 'Add Task'}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <div className="mx-auto col-10 col-md-8 col-lg-6">
                    <Form onSubmit={handleSubmit}>
                        <Form.Group className="mb-3" controlId="formBasicEmail">
                            <Form.Label>Name</Form.Label>
                            <Form.Control
                                type="text"
                                name="Name"
                                value={task.Name}
                                onChange={handleChange}

                            />
                            </Form.Group>

                            <Form.Group className="mb-3" controlId="formBasicEmail">
                                <Form.Label>Description</Form.Label>
                                <Form.Control
                                    type="text"
                                    name="Description"
                                    value={task.Description}
                                    onChange={handleChange}

                                />
                            </Form.Group>

                            <Form.Group className="mb-3" controlId="formBasicEmail">
                                <Form.Label>Start Date</Form.Label>
                                <Form.Control
                                    type="date"
                                    name="StartDate"
                                    value={task.StartDate}
                                    onChange={handleChange}
                                />
                            </Form.Group>

                            <Form.Group className="mb-3" controlId="formBasicEmail">
                                <Form.Label>End Date</Form.Label>
                                <Form.Control
                                    type="date"
                                    name="EndDate"
                                    value={task.EndDate}
                                    onChange={handleChange}
                                />
                            </Form.Group>

                            <Form.Group>
                                <Form.Label>User</Form.Label>
                                {loading ? <p><em>Loading...</em></p>
                                    :
                                <Form.Select
                                    name="UserId"
                                    value={task.UserId}
                                    onChange={handleChange}>
                                        <option value={0}>None</option>
                                    {users.map((user) => (

                                        <option key={user.userId} value={user.userId}>{user.firstName} {user.lastName}</option>

                                    ))}
                                </Form.Select>}
                            </Form.Group>

                            <Form.Group>
                                <Form.Label>Task Status</Form.Label>
                                {loading ? <p><em>Loading...</em></p>
                                    :
                                <Form.Select
                                    name="StatusId"
                                    value={task.StatusId}
                                        onChange={handleChange}>                                       
                                        <option value={0}>None</option>
                                    {statuses.map((s) => (

                                        <option key={s.statusId} value={s.statusId}>{s.name}</option>

                                    ))}
                                </Form.Select>}
                            </Form.Group>        

                            <Form.Check name="IsComplete" type="switch" id="complete" label="Complete"
                                onChange={handleComplete} checked={task.IsComplete} value={complete}></Form.Check>

                            <Form.Group>
                                <Form.Label>Prerequisite Task</Form.Label>
                                {loading ? <p><em>Loading...</em></p>
                                    :
                                    <Form.Select
                                        name="PrereqTaskId"
                                        value={task.PrereqTaskId}
                                        onChange={handleChange}>
                                        <option value={0}>None</option>
                                        {tasks.map((aTask) => (

                                            <option key={aTask.taskId} value={aTask.taskId}>{aTask.name }</option>

                                        ))}
                                    </Form.Select>}
                            </Form.Group>       
                            
                        </Form>                           
                        </div>

                </Modal.Body>
                <Modal.Footer>
                    <button onClick={props.handleClose}>
                        Close
                    </button>
                    <button type="submit" onClick={handleSubmit}>
                        {props.edit ? 'Update' : 'Add'}
                    </button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default CreateTask;
