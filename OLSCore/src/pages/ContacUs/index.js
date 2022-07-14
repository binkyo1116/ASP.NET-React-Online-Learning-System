import React, { useState } from 'react'
import { Link } from 'react-router-dom';
import './ContactUs.css'
import logoInline from './../../assets/images/logo-inline.png'
import InputField from '../../components/Common/InputField/InputField';
import { API_URL } from '../../Global';
import UserIcon from '../../assets/icons/userIcon.svg';
import InboxIcon from '../../assets/icons/inbox.svg';
import MessageIcon from '../../assets/icons/message.svg';
import SMSIcon from '../../assets/icons/SMS1.svg';
import TextareaField from '../../components/Common/TextareaField/TextareaField';

function ContactUs() {

	const [username, setUsername] = useState('')
	const [usernameError, setUsernameError] = useState('')
	const [email, setEmail] = useState('')
	const [emailError, setEmailError] = useState('')
	const [message, setMessage] = useState('')
	const [messageError, setMessageError] = useState('')

	const sendMessage = () => {
		const data = {
			SenderName: username,
			SenderEmail: email,
			Message: message
		};
		if (validateFields()) {
			fetch(`${API_URL}ContactUs`, {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json',
				},
				body: JSON.stringify(data),
			})
				.then(response => response.json())
				.then(jsonResponse => {
					console.log('Success: ', jsonResponse)
					const error = jsonResponse.error
					if (error === "Sorry! Your Email address is not valid. Please provide a valid email address.") {
						setUsernameError('')
						setEmailError('Invalid Email')
					}
					if (error === "Sorry! The Name field cannot be left blank.") {
						setUsernameError('Name field cannot be empty')
						setEmailError('')
					}
					if (jsonResponse.success === "Your Message has been sent successfully."){
						setUsername('')
						setEmail('')
						setMessage('')
						setUsernameError('')
						setEmailError('')
						setMessageError('')
					}
				})
				.catch(error => console.log('Error: ', error));
		}
	}
	const validateFields = () => {
		let is_valid = true;
		const reg = /^[a-zA-Z0-9]+@(?:[a-zA-Z0-9]+\.)+[A-Za-z]+$/
		if (!username) {
			setUsernameError('Name field cannot be empty')
			is_valid = false;
		}
		else setUsernameError('')
		if (!email) {
			setEmailError('Email field cannot be empty')
			is_valid = false;
		}
		else if (!reg.test(email)) {
			setEmailError('Invalid Email')
			is_valid = false;
		}
		else setEmailError('')
		if (!message) {
			setMessageError('Message field cannot be empty')
			is_valid = false;
		}
		else setMessageError('')
		return is_valid
	}

	return (
		<div className='bg-contatiner'>
			<div className='bg-hero'>
				<div className='contactus-section'>
					<div className='contactus-container'>
						<div className='custom-card over-section'>
							<div>
								<h2 className='text-uppercase font-28'>Contact Form</h2>
							</div>
							<div className='mb-3'>
								<InputField
									type='text'
									placeholder="Your Name"
									leftIcon={UserIcon}
									error={usernameError}
									className="py-4"
									value={username}
									onChange={(e) => setUsername(e.target.value)}
								/>
								<InputField
									type='email'
									placeholder="Your Email"
									leftIcon={InboxIcon}
									error={emailError}
									className="py-4"
									value={email}
									onChange={(e) => setEmail(e.target.value)}
								/>
								<div className="py-4">
									<img src={MessageIcon} alt="" />
									<span className='pl-2 text-cus-secondary'>Message</span>
								</div>
								<TextareaField
									className='textarea-cus'
									placeholder='Enter Your Message'
									value={message}
									onChange={(e) => setMessage(e.target.value)}
									error={messageError}
								/>
							</div>
							<div className='d-flex align-items-center justify-content-between pt-4'>
								<button className='btn-cus-primary w-100' onClick={sendMessage}>Send Message</button>
							</div>
						</div>

						<div className='wide-section py-3'>
							<div className='back-login'>
							<img src={logoInline} alt='logo' className='logo-image' />
							</div>
							<div className='mb-5'>
								<h3 className='font-24 text-white mb-2'>Get in touch!</h3>
								<p className='font-20 text-cus-secondary'>Fill the form or send as an email</p>
							</div>
							<div className='wide-btn-group gap-4 pt-4'>
								<Link to='#' 
									onClick={(e)=>{
										window.location.href = 'mailto:help@letsusedata.com';
										e.preventDefault();
									}}
									className='btn-cus-secondary px-5'
								>Email Us</Link>
								<Link to='/login' className='link-cus-secondary login'>Login</Link>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	)
}

export default ContactUs