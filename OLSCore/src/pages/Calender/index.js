import React from 'react'
import SideBar from '../../components/SideBar';
import CalenderMain from './calender';
import './calender.scss';
import CalenderRight from './CalenderRight';
function Calender() {
  return (
    <div className='calender-container'>
        <SideBar activeCalender/>
        <CalenderMain />
        <CalenderRight />
    </div>
  )
}

export default Calender
