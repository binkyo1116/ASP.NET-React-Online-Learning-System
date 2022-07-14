import React, { Component } from 'react'
import './Sidebar.scss'
import mainLogo from '../../../assets/images/logo.svg'
import listIcon1 from '../../../assets/icons/dashboard.svg' 
import listIcon2 from '../../../assets/icons/calender.svg'
import listIcon3 from '../../../assets/icons/double-message.svg' 
import listIcon4 from '../../../assets/icons/info.svg'
import contactInfo from '../../../assets/icons/chat-info.svg'

export default class index extends Component {
  render() {
    return (
      <div className='col-2 sidebar-container'>
      <div className='sidebar-logo-container'>
          <img src={mainLogo} alt='' />
      </div>
      <div className='sidebar-menu-container'>
        <div>
          <label className='sidebar-menu-label'>Personal</label>
          <ul className='sidebar-list-container'>
            <li className='active'><div className='list-icon'><img src={listIcon1} alt="" /></div>Dashboard</li>
            <li><div className='list-icon'><img src={listIcon2} alt="" /></div> Calendar</li>
            <li><div className='list-icon'><img src={listIcon3} alt="" /></div> Message</li>
            <li><div className='list-icon'><img src={listIcon4} alt="" /></div> Help Center</li>
          </ul>
        </div>
      </div>
      <div className='contact-info-container'>
        <div className=''>
          <div className='contact-icon-container'>
            <img src={contactInfo} alt="" />
          </div>
          
          <div className='text-center'>
            <label className='helper-text'>Need some help ?</label>
            <button className='btn-fill'>Contact US</button>
          </div>
        </div>
        <hr  className='dark-mode-border'/>
        <div>
          <button className='btn-outline'>Discover Tutorial</button>
        </div>
      </div>
    </div>
    )
  }
}
