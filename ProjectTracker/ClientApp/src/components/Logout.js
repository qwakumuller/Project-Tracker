import { useCallback, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { NotificationManager } from "react-notifications";
import Cookies from "js-cookie";

export default function Logout() {
  const userCookie = Cookies.get("userId");
  const navigate = useNavigate();

  const userLogout = useCallback(async () => {
    await fetch("user/logout", {
      method: "DELETE",
      headers: {
        "Content-type": "application/json; charset=UTF-8",
      },
    })
      .then(async (response) => {
        if (response.ok) {
          await response;
          console.log(response.text());
          console.log("This is the cookie after logout" + userCookie);
          //Put a check for some browsers, that still keep cookie in cache
          if (userCookie !== null) {
            console.log("Browser still has it");
            Cookies.remove("userId");
          }
          NotificationManager.success("Successfully Logout");
          navigate("/login");
        } else {
          console.log(response);
          if (response.status === 404 || response.status === 503) {
            NotificationManager.error("Service is Unavailable");
          } else {
            NotificationManager.error("Error occured");
          }
        }
      })
      .catch((err) => {
        NotificationManager.error("Service Unavailable");
        console.log("error " + err);
      });
  }, [userCookie, navigate]);

  useEffect(() => {
    userLogout();
  }, [userLogout]);  
  
}
