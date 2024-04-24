import React, {useState} from 'react';
import { useNavigate } from "react-router-dom";
import validator from 'validator'
import './CreateAccount.css';

export default function CreateAccount() {
    const [account, setAccount] = useState({
        FirstName: "",
        LastName: "",
        Email: "",
        Password: "",
        IsAdmin: false
    });

    const navigate = useNavigate();
    const [errorMessage, setErrorMessage] = useState("");
    const [emailError, setEmailError] = useState("");
    const [passwordError, setPasswordError] = useState("");

    const handleChange = (e) => {
        setAccount({
            ...account,
            [e.target.id]: e.target.value
        })
    }

    function handleSubmit(e) {
        e.preventDefault();
        createAccount();
    }

    const validateEmail = (e) => {
        var email = e.target.value
        setErrorMessage("");
        handleChange(e);

        if (validator.isEmail(email)) {
        setEmailError('');
        } else {
        setEmailError('Invalid email!');
        }
    }

    const validatePassword = (e) => {
        var password = e.target.value
        handleChange(e);
        let res = "";
        setPasswordError("");      
        
        if (!validator.isStrongPassword(password, {minLength: 8,minLowercase: 0,minUppercase: 0,minNumbers: 0,minSymbols: 0})) {
            res = `- Password should be at least 8 characters\n`;
        }
        if (!validator.isStrongPassword(password, {minLength: 0,minLowercase: 1,minUppercase: 0,minNumbers: 0,minSymbols: 0})) {
            res += `- Password should contains at least 1 lower case\n`;
        }
        if (!validator.isStrongPassword(password, {minLength: 0,minLowercase: 0,minUppercase: 1,minNumbers: 0,minSymbols: 0})) {
            res += `- Password should contains at least 1 upper case\n`;
        }
        if (!validator.isStrongPassword(password, {minLength: 0,minLowercase: 0,minUppercase: 0,minNumbers: 1,minSymbols: 0})) {
            res += `- Password should contains at least 1 number\n`;
        }       
        
        if (!validator.isStrongPassword(password, {minLength: 0,minLowercase: 0,minUppercase: 0,minNumbers: 0,minSymbols: 1})) {
            res += '- Password should contains at least 1 symbol\n';
        }
        setPasswordError(res);
      }

    async function createAccount() {
        await fetch('user', {
            method: "POST",
            body: JSON.stringify(account),
            headers: {
                'Content-type': 'application/json; charset=UTF-8',
            }
        })
        .then(async (res) => {
            if (res.ok){
                await res.json();
                navigate("/");                
            } else {
                const msg = await res.text()
                console.log(msg);
                setErrorMessage(msg);
            }
        })
        .catch(exc => {
            console.log("exc " + exc);
        })
    }

    return (
      <div className="row">
        <div className="mx-auto col-10 col-md-8 col-lg-6">
            <h1>Sign Up</h1>
            <form onSubmit={handleSubmit}>
                <div className="form-row mb-3">
                    <div className="form-group col-md-12">
                        <label htmlFor="FirstName">First Name:</label>
                        <input id="FirstName" className="form-control col-sm-10" type="text" placeholder="First Name" value={account.FirstName} onChange={e => handleChange(e)} required/>
                    </div>
                </div>

                <div className="form-row mb-3">
                    <div className="form-group col-md-12">
                        <label htmlFor="LastName">Last Name:</label>
                        <input id="LastName" className="form-control" type="text" placeholder="Last Name" value={account.LastName} onChange={e => handleChange(e)} required/>
                    </div>
                </div>

                <div className="form-row mb-3">
                    <div className="form-group col-md-12">
                        <label htmlFor="Email">Email:</label>
                        <input id="Email" type="email" className="form-control" placeholder="name@example.com" value={account.Email} onChange={e => validateEmail(e)} required/>
                    </div>
                </div>

                <div className="form-row mb-3">
                    <div className="form-group col-md-12">
                        <label htmlFor="Password">Password:</label>
                        <input id="Password" className="form-control" type="password" placeholder="Password" value={account.Password} onChange={e => validatePassword(e)} required/>
                    </div>
                </div>
                <button className="btn btn-primary" block="true" size="lg" type="submit">Submit</button>

                {errorMessage.length > 0 && <div className="error mt-2">{errorMessage}</div>}
                {emailError.length > 0 && <div className="error mt-2">{emailError}</div>}
                {passwordError.length > 0 && <div className="error mt-2 break_space">{passwordError}</div>}
            </form>
        </div>
      </div>
    );
  
}
