import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import Form from 'react-bootstrap/Form'
import { NotificationManager } from 'react-notifications'
import { NavLink } from "reactstrap";

export default function Login() {
    const [login, setLogin] = useState({ email: "", password: "" });
    const navigate = useNavigate();

    const handleSubmit = (event) => {
        event.preventDefault();
        userLogin();
    }

    const handleChange = (event) => {
        const { name, value } = event.target;
        setLogin((prevState) => ({
            ...prevState, [name]: value,
        }))
    }

    async function userLogin() {
        await fetch('user/login', {
            method: "POST",
            body: JSON.stringify(login),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
            }
        })
            .then(async (response) => {
                if (response.ok) {
                    await response
                    NotificationManager.success("Successfully login");
                    navigate("/");
                } else {
                    console.log(response)
                    if (response.status === 404 || response.status === 503) {
                        NotificationManager.error("Service is Unavailable")
                    } else if (response.status === 401 || response.status === 403) {
                        NotificationManager.error("Email and Password Incorrect");
                    } else {
                        NotificationManager.error("Error occured")
                    }
                }
            })
            .catch(err => {
                NotificationManager.error("Service Unavailable")
                console.log("error " + err);
            })
    }

    return (
        <div className="mx-auto col-10 col-md-8 col-lg-6">
            <h1>LOGIN</h1>
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3" controlId="Email">
                    <Form.Label>Email</Form.Label>
                    <Form.Control
                        type="text"
                        name="email"
                        value={login.email}
                        onChange={handleChange}
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="Password">
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                        type="password"
                        name="password"
                        value={login.password}
                        onChange={handleChange}
                    />
                </Form.Group>

                <button className="btn btn-primary btn-sm" type="submit">
                    LOGIN
                </button>

                <button className="ms-1 btn btn-primary btn-sm">
                    <NavLink tag={Link} to="/create-account">Create Account</NavLink>
                </button>

            </Form>

        </div>
    )
}