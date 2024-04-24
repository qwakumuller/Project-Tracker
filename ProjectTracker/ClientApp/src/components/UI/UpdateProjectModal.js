import React, { useState, useEffect } from 'react';
import classes from './UpdateProjectModal.module.css';
import { Modal, Button, Form } from 'react-bootstrap';

export default function UpdateProjectModal({ isOpen, toggleModal, projId }) {
    const [project, setProject] = useState([]);
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [startDate, setStartDate] = useState("");
    const [endDate, setEndDate] = useState("");
    const [isSuccess, setSuccess] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const [timer, setTimer] = useState(0);

    const submitHandler = (event) => {
        event.preventDefault();
        setSuccess("");
        setErrorMessage("");
        const data = {
            projectId: projId,
            name: name,
            description: description,
            startDate: startDate,
            endDate: endDate
        }
        fetch('api/project', {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(data),
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error("Error: " + response.statusText);
                }
                return response.json();
            })
            .then((data) => {
                setSuccess("Successful Update");
                successHandler();
            })
            .catch((error) => {
                setErrorMessage(error);
                errorHandler();
            });
    };

    useEffect(() => {
        if (isOpen) {
            setErrorMessage("");
            fetch(`api/project/${projId}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error("Error: " + response.statusText);
                    }
                    return response.json();
                })
                .then(data => {
                    setProject(data);
                })
                .catch(error => {
                    setErrorMessage(error);
                    errorHandler();
                });
        }
    }, [isOpen, projId]
    );

    useEffect(() => {
        setName(project.name);
        setDescription(project.description);
        setStartDate(project.startDate);
        setEndDate(project.endDate);
    }, [project]
    );

    const successHandler = () => {
        const timerId = setTimeout(() => {
            setSuccess("");
        }, 2000);
        setTimer(timerId);
    }

    const errorHandler = () => {
        const timerId = setTimeout(() => {
            setErrorMessage("");
        }, 4000);
        setTimer(timerId);
    }

    useEffect(() => {
        return () => {
            clearTimeout(timer);
        };
    }, [timer]
    );

    return (
        <div>
            <Modal show={isOpen} onHide={toggleModal}>
                <Modal.Header>
                    <Modal.Title>Update Project {projId}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form onSubmit={submitHandler} id={"form"}>
                        <Form.Group controlId={"name"}>
                            <Form.Label>Name:</Form.Label>
                            <Form.Control type={"text"} defaultValue={project?.name}
                                onChange={(e) => setName(e.target.value)} />
                        </Form.Group>
                        <Form.Group controlId={"description"}>
                            <Form.Label>Description:</Form.Label>
                            <Form.Control type={"text"} as="textarea" rows={3} defaultValue={project?.description}
                                onChange={(e) => setDescription(e.target.value)} />
                        </Form.Group>
                        <Form.Group controlId={"startDate"}>
                            <Form.Label>Start Date:</Form.Label>
                            <Form.Control type={"text"} defaultValue={project?.startDate?.substring(0, 10)}
                                onChange={(e) => setStartDate(e.target.value)} />
                        </Form.Group>
                        <Form.Group controlId={"endDate"}>
                            <Form.Label>End Date:</Form.Label>
                            <Form.Control type={"text"} defaultValue={project?.endDate?.substring(0, 10)}
                                onChange={(e) => setEndDate(e.target.value)} />
                        </Form.Group>
                    </Form>
                </Modal.Body>
                <Modal.Footer>
                    {isSuccess && <p className={classes.successMsg}>{isSuccess}</p>}
                    {errorMessage && <p className={classes.errorMsg}>{errorMessage}</p>}
                    <Button variant="danger" onClick={toggleModal}>Close</Button>
                    <Button type={"submit"} form={"form"} variant="secondary">Submit</Button>
                </Modal.Footer>
            </Modal>
        </div>
    )
}