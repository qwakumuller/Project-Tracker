import React, { useEffect, useState } from "react";
import {
  Collapse,
  Navbar,
  NavbarBrand,
  NavbarToggler,
  NavItem,
  NavLink,
} from "reactstrap";
import { Link, useNavigate } from "react-router-dom";
import Cookies from "js-cookie";
import "./NavMenu.css";

export default function NavMenu() {
  const [collapsed, setCollapsed] = useState(true);
  const userCookie = Cookies.get("userId");
  const navigate = useNavigate();

  useEffect(() => {
    console.log("This is the token " + userCookie);
    if ((userCookie !== null && userCookie !== undefined) && (window.location.pathname === '/login' || window.location.pathname === '/create-account'))
      navigate("/");

    if ((userCookie === null || userCookie === undefined) && window.location.pathname !== '/create-account') {
      console.log("Cookie is not  available");
      navigate("/login");
    }
  }, [userCookie, navigate]);

  return (
    <header>
      <Navbar
        className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
        container
        light
      >
        {userCookie === null || userCookie === undefined ? (
          <NavbarBrand>ProjectTracker</NavbarBrand>
        ) : (
          <NavbarBrand tag={Link} to="/">
            ProjectTracker
          </NavbarBrand>
        )}
        <NavbarToggler
          onClick={() => setCollapsed(!collapsed)}
          className="mr-2"
        />
        <Collapse
          className="d-sm-inline-flex flex-sm-row-reverse"
          isOpen={collapsed}
          navbar
        >
          <ul className="navbar-nav flex-grow">
            {userCookie === null || userCookie === undefined ? (
              <NavItem>
                <button className="btn btn-primary btn-sm">
                  <NavLink tag={Link} className="text-dark" to="/login">
                    Login
                  </NavLink>
                </button>
              </NavItem>
            ) : (
              ""
            )}

            {userCookie === null || userCookie === undefined ? (
                ""
            ) : (
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/create-Project">
                    Create Project
                  </NavLink>
                </NavItem>
            )}

            {userCookie === null || userCookie === undefined ? (
              ""
            ) : (
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/">
                  Home
                </NavLink>
              </NavItem>
            )}
            {/* {userCookie === null || userCookie === undefined ? (
              ""
            ) : (
              <NavItem>
                <NavLink tag={Link} className="text-dark" to="/counter">
                  Counter
                </NavLink>
              </NavItem>
            )} */}
            {userCookie === null || userCookie === undefined ? (
              ""
            ) : (
              <NavItem>
                <button type="button" className="btn btn-danger btn-sm">
                  <NavLink tag={Link} className="text-dark" to="/logout">
                    Logout
                  </NavLink>
                </button>
              </NavItem>
            )}
          </ul>
        </Collapse>
      </Navbar>
    </header>
  );
}
