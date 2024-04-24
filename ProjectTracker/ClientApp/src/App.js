import React from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import Layout from './components/Layout';
import './custom.css';
import {NotificationContainer} from 'react-notifications'
import 'react-notifications/lib/notifications.css';
import 'bootstrap/dist/css/bootstrap.min.css'

export default function App() {

    return (
      <Layout>
        <Routes>
          {AppRoutes.map((route, index) => {
            const { element, ...rest } = route;
            return <Route key={index} {...rest} element={element} />;
          })}
        </Routes>
        <NotificationContainer/>
      </Layout>
    );
}
