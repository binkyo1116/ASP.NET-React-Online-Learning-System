import React, { useState } from "react";
import { Link } from "react-router-dom";
import "./login.css";
import logoInline from "./../../assets/images/logo-inline.png";
import InputField from "../../components/Common/InputField/InputField";
import { API_URL } from "../../Global";
import EmailIcon from "../../assets/icons/userIcon.svg";
import LockIcon from "../../assets/icons/keyIcon.svg";

function Login(props) {
  const [email, setEmail] = useState("");
  const [emailError, setEmailError] = useState("");
  const [password, setPassword] = useState("");
  const [passwordError, setPasswordError] = useState("");

  const login = () => {
    const data = {
      username: email,
      password: password,
    };
    fetch(`${API_URL}login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    })
      .then((response) => response.json())
      .then((jsonResponse) => {
        console.log("Success: ", jsonResponse, props);
        const error = jsonResponse.error;
        const guid = jsonResponse.studentIdHash;
        if (guid === "-1") {
          if (error === "Password was incorrect") {
            setEmailError("");
            setPasswordError("Incorrect Password");
          }
          if (error === "Could not find a login with that username") {
            setEmailError("Invalid User");
            setPasswordError("");
          }
        } else {
          localStorage.setItem("loginData", JSON.stringify(jsonResponse));
          localStorage.setItem("KeepMeLoggedIn", true);
          localStorage.setItem("Hash", guid);
          localStorage.setItem("StudentName", jsonResponse.StudentName);
          localStorage.setItem("StudentProfileImage", jsonResponse.Picture);

          if (jsonResponse.IsAdmin & (jsonResponse.AdminHash !== "")) {
            localStorage.setItem("IsAdmin", jsonResponse.IsAdmin);
            localStorage.setItem("AdminHash", jsonResponse.AdminHash);
          }
          props.history.go("/dashboard");
        }
      })
      .catch((error) => console.log("Error: ", error));
  };

  return (
    <div className="bg-contatiner">
      <div className="bg-hero">
        <div className="login-section">
          <div className="login-container">
            <div className="custom-card over-section">
              <div>
                <h2 className="text-uppercase font-28">Log In</h2>
                <p className="font-18 text-cus-secondary pb-4">
                  Using Data to Improve your Learning Experience
                </p>
              </div>
              <div>
                <InputField
                  type="email"
                  placeholder="Email"
                  leftIcon={EmailIcon}
                  error={emailError}
                  className="py-4"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
                <InputField
                  type="password"
                  placeholder="Password"
                  leftIcon={LockIcon}
                  hasRightIcon={true}
                  showPassIcon={true}
                  error={passwordError}
                  className="py-4"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
                <div className="keep-logged-in text-cus-secondary pt-2">
                  <input
                    type="checkbox"
                    className="form-check-input"
                    id="keepLoggedIn"
                  />
                  <label className="pl-2 mb-0" htmlFor="keepLoggedIn">
                    Remembered Me
                  </label>
                </div>
              </div>
              <div className="d-flex align-items-center justify-content-between pt-5">
                <Link
                  to="/forgotpassword"
                  className="link-cus-primary text-capitalize"
                >
                  forgot password ?
                </Link>
                <button className="btn-cus-primary px-5" onClick={login}>
                  SIGN IN
                </button>
              </div>
            </div>

            <div className="wide-section py-3">
              <img src={logoInline} alt="logo" className="logo-image" />
              <div className="mb-5">
                <h3 className="font-24 text-white mb-2">
                  Don’t have account ?
                </h3>
                <p className="font-18 text-cus-secondary">
                  Use the request login button to ask your instructor for one
                </p>
              </div>
              <div className="wide-btn-group gap-4">
                <Link to="/requestlogin" className="btn-cus-secondary px-5">
                  REQUEST LOGIN
                </Link>
                <Link to="/contactus" className="link-cus-secondary contact-us">
                  Contact Us
                </Link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Login;
