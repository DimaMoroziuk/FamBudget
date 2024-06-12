import React, { useState, useContext, useEffect } from 'react';
import { useHistory, useParams, Link } from "react-router-dom";
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { useUser, userContext } from '../contexts/userContext'
import './NavMenu.css';

export default function NavMenu() {
  const history = useHistory ();
  const [collapsed, setCollapsed] = useState(true);
  const [navItems, setNavItems] = useState(<></>);
  const { user, setUser, setUserData } = useUser();


  // const user = useContext(userContext);

  useEffect(() => {
    setNavItems(getNavbarItems());
}, [user]);

  const logOut = () => {
    fetch(`api/Account/Logout`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' }
    })
    .then(response => history.push("/"));
  }
  
  const toggleNavbar = () => {
    setCollapsed(!collapsed);
  }

  const getNavbarItems = () => {
    if(user && Object.keys(user).length)
      {
        return <><NavItem>
        <NavLink tag={Link} className="text-dark" to="/purchases">Purchases</NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/incomes">Incomes</NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/categories">Categories</NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/reporting">Reports</NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/accesses">Access Manager</NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" onClick={logOut}>Log out</NavLink>
      </NavItem>
      </>;

      } else {
        return <><NavItem>
        <NavLink tag={Link} className="text-dark" to="/registration">Sing Up</NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/login">Sign In</NavLink>
      </NavItem></>;
      }
  }

    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">FamBudget</NavbarBrand>
            <NavbarToggler onClick={toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
              <ul className="navbar-nav flex-grow">
              {navItems}
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
}
