import CreateAccount from "./components/CreateAccount";
import Home from "./components/Home";
import Login from "./components/Login";
import CreateProject from "./components/CreateProject";
import Logout from "./components/Logout";
import Project from "./components/Project";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/create-account',
    element: <CreateAccount />
  },
  {
    path: '/login',
    element: <Login />
  },
  {
    path: '/create-project',
    element: <CreateProject />
  },
  {
    path: '/logout',
    element: <Logout />
  },
  {
    path: '/project/:id',
    element: <Project />
  }
];

export default AppRoutes;
