import React, { useState } from 'react'
import { Link } from 'react-router-dom';
import './RequestLogin.css'
import logoInline from './../../assets/images/logo-inline.png'
import InputField from '../../components/Common/InputField/InputField';
import { API_URL } from '../../Global';
import UserIcon from '../../assets/icons/userIcon.svg';
import InboxIcon from '../../assets/icons/inbox.svg';
import HomeIcon from '../../assets/icons/home.svg';
import NumericalStarIcon from '../../assets/icons/numerical-star.svg';

function RequestLogin() {

	const [username, setUsername] = useState('')
	const [usernameError, setUsernameError] = useState('')
	const [email, setEmail] = useState('')
	const [emailError, setEmailError] = useState('')
	const [schoolName, setSchoolName] = useState('')
	const [schoolNameError, setSchoolNameError] = useState('')
	const [courseName, setCourseName] = useState('')
	const [courseNameError, setCourseNameError] = useState('')

	const sendMessage = () => {
		const data = {
			Name: username,
			Email: email,
			SchoolName: schoolName,
			CourseName: courseName
		};
		if (validateFields()) {
			fetch(`${API_URL}StudentRequestLogin`, {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json',
				},
				body: JSON.stringify(data),
			})
				.then(response => response.json())
				.then(jsonResponse => {
					console.log('Success: ', jsonResponse)
				})
				.catch(error => console.log('Error: ', error));
		}
	}
	const validateFields = () => {
		let is_valid = true;
		const reg = /^[a-zA-Z0-9]+@(?:[a-zA-Z0-9]+\.)+[A-Za-z]+$/
		if (!schoolName) {
			setSchoolNameError('School name cannot be empty')
			is_valid = false;
		}
		else setSchoolNameError('')
		if (!courseName) {
			setCourseNameError('Course name cannot be empty')
			is_valid = false;
		}
		else setCourseNameError('')
		if (!username) {
			setUsernameError('Name cannot be empty')
			is_valid = false;
		}
		else setUsernameError('')
		if (!email) {
			setEmailError('Email cannot be empty')
			is_valid = false;
		}
		else if (!reg.test(email)) {
			setEmailError('Invalid Email')
			is_valid = false;
		}
		else setEmailError('')
		return is_valid
	}

	return (
		<div className='bg-contatiner'>
			<div className='bg-hero'>
				<div className='request-section'>
					<div className='request-container'>
						<div className='custom-card over-section'>
							<div>
								<h2 className='text-uppercase font-28'>REQUEST LOGIN</h2>
								<p className='text-cus-secondary font-18'>Input your information below</p>
							</div>
							<div className='mb-3'>
								<InputField
									type='text'
									placeholder="Your School Name"
									leftIcon={HomeIcon}
									error={schoolNameError}
									className="py-44"
									value={schoolName}
									onChange={(e) => setSchoolName(e.target.value)}
								/>
								<InputField
									type='text'
									placeholder="Your Course Name"
									leftIcon={NumericalStarIcon}
									error={courseNameError}
									className="py-44"
									value={courseName}
									onChange={(e) => setCourseName(e.target.value)}
								/>
								<InputField
									type='text'
									placeholder="Your Full Name"
									leftIcon={UserIcon}
									error={usernameError}
									className="py-44"
									value={username}
									onChange={(e) => setUsername(e.target.value)}
								/>
								<InputField
									type='email'
									placeholder="Your Email"
									leftIcon={InboxIcon}
									error={emailError}
									className="py-44"
									value={email}
									onChange={(e) => setEmail(e.target.value)}
								/>
								<div className="keep-logged-in text-cus-secondary">
									<input type="checkbox" className="form-check-input" id="keepLoggedIn" />
									<label className="pl-2 mb-0" htmlFor="keepLoggedIn">Remembered Me</label>
								</div>
							</div>
							<div className='d-flex align-items-center justify-content-between pt-4'>
								<button className='btn-cus-primary w-100' onClick={sendMessage}>SUBMIT REQUEST</button>
							</div>
						</div>

						<div className='wide-section py-3'>
							<div className='back-login'>
							<img src={logoInline} alt='logo' className='logo-image' />
							</div>
							<div className='mb-5'>
								<h3 className='font-24 text-white mb-2'>Already have an account ?</h3>
								<p className='font-18 text-cus-secondary'>Go back to the login screen and enter your username with password</p>
							</div>
							<div className='wide-btn-group gap-4'>
								<Link to='/login'
									className='btn-cus-secondary px-5'
								>LOGIN</Link>
								<Link to='/contactus' className='link-cus-secondary login'>Contact Us</Link>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	)
}

export default RequestLogin