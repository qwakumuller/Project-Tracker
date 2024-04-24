import React, {useState, useEffect, useCallback} from 'react';
import './Home.css';
import Cookies from "js-cookie";
import { Table } from "reactstrap";
import { useNavigate } from "react-router-dom";

export default function Home() {

    const [projects, setProjects] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();
    const encodedId = encodeURIComponent(Cookies.get("userId"));

    const getUser = useCallback(async () => {
        await fetch(`api/project/user/${encodedId}`)
            .then(async (response) => {
                if (!response.ok) {
                    throw new Error('Error when fetching data.')
                }
                const data = await response.json();
                return data;
            }) 
            .then(data => {
                setProjects(data);
                setLoading(false);
            })
            .catch(error => {
                //setError(error.message);
                console.log(error);
            });
        setLoading(false);
    }, [encodedId]);

    // const toggleModal = (event) => {
    //     event.preventDefault();
    //     setOpen(!isOpen);
    //     return false;
    // };

    useEffect(() => {
        setLoading(true);
        //setError(null);
        getUser();

    }, [getUser, setLoading]);
    
    const rowClickHandler = (id) => {
        navigate(`/project/${id}`);
    };

    return (

        <div>
            {loading ? <p><em>Loading...</em></p> :
                <Table className="table table-striped table-hover user-projects-table">
                    <thead className="border-dark">
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Start Date</th>
                        <th>End Date</th>
                    </tr>
                    </thead>
                    <tbody>
                    {projects.map(project => (
                        <tr key={project.projectId} className="project-row" onClick={() => rowClickHandler(project.projectId)}>
                            <td>{project.name}</td>
                            <td>{project.description}</td>
                            <td>{project.startDate.substring(0, 10)}</td>
                            <td>{project.endDate.substring(0, 10)}</td>
                        </tr>
                    ))}
                    </tbody>
                </Table>}
        </div>
    );
}
