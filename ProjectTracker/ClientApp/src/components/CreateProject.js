import { useState } from "react";
import Form from 'react-bootstrap/Form'
import { NotificationManager } from 'react-notifications'
import Cookies from "js-cookie";

export default function CreateProject() {
    const [project, setProject] = useState({ Name: "", Description: "" , StartDate:"", EndDate:""});
    var projectId = "";
    const userId = Cookies.get("userId");

    const handleSubmit = (event) => {
        event.preventDefault();
       createProject();
    }

    const handleChange = (event) => {
        const { name, value } = event.target;
        setProject((prevState) => ({
            ...prevState, [name]: value,
        }))
    }

    async function createProject() {
        await fetch('api/project', {
            method: "POST",
            body: JSON.stringify(project),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
            }
        })
            .then(async (response) => {
                if (response.ok) {
                    const project = await response.json();
                    projectId = project.projectId;
                    addUserToProject();
                    //NotificationManager.success("Project Successfully Created");
                } else {
                    NotificationManager.error("Project Creation UnSuccessful");
                }
            })
            .catch(err => {
                NotificationManager.error("Service Unavailable")
                console.log("error " + err);
            })

    }

    async function addUserToProject() {
        const user = ({ProjectId: projectId, UserId: userId})
        await fetch('api/userproject', {
            method: "POST",
            body: JSON.stringify(user),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
            }
        })
            .then(async (res) => {
                if (res.ok) {
                    await res.json();
                    NotificationManager.success("Project Successfully Created");
                } else {
                    NotificationManager.error("Project Creation UnSuccessful");
                }
            })
            .catch(err => {
                NotificationManager.error("Service Unavailable")
                console.log("error " + err);
            })

    }
    


    return (
        <div className="mx-auto col-10 col-md-8 col-lg-6">
            <h1>Create Project</h1>
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3" controlId="Name">
                    <Form.Label>Name</Form.Label>
                    <Form.Control
                        type="text"
                        name="Name"
                        value={project.Name}
                        onChange={handleChange}
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="Description">
                    <Form.Label>Description</Form.Label>
                    <Form.Control
                        type="text"
                        name="Description"
                        value={project.Description}
                        onChange={handleChange}
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="StartDate">
                    <Form.Label>StartDate</Form.Label>
                    <Form.Control
                        type="date"
                        name="StartDate"
                        value={project.StartDate}
                        onChange={handleChange}
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="EndDate">
                    <Form.Label>EndDate</Form.Label>
                    <Form.Control
                        type="date"
                        name="EndDate"
                        value={project.EndDate}
                        onChange={handleChange}
                    />
                </Form.Group>

                <button variant="primary" type="submit">
                    Create Project
                </button>

            </Form>
        </div>
    )

}