import React from "react";
import SideBar from "./sidebar";
import { Route, Switch, Redirect } from "react-router-dom";
import Users from "./users";
import Posts from "./posts";

const Dashboard = ({ match }) => {
  return (
    <div>
      <h1>Admin Dashboard</h1>
      <SideBar />
      <Switch>
        <Route path="/admin/users" component={Users} />
        <Route path="/admin/posts" component={Posts} />
        <Redirect to="/not-found" />
      </Switch>
    </div>
  );
};

export default Dashboard;
