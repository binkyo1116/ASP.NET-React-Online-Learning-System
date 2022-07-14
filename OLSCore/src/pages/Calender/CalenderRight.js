import React, {useState} from 'react';
import './calender.scss';
import userIcon from '../../assets/icons/userIcon.svg';
import chevronDown from '../../assets/icons/chevron-down.svg';
import warning from '../../assets/icons/warning.svg'
import Calendar from 'react-calendar';
import chatInfo from '../../assets/icons/Single-chat.svg';
import info from '../../assets/icons/info.svg'

function CalenderRight() {
    const [isVisible, setIsVisible] = useState(false);
  return (
    <div className='calender__right-head'>
        <div className='right--user'>
            <span className='calender__right-user'>
                <img src={userIcon} alt="use pic" className='user--avatar'/>
            </span>
            <div className='user_name_email'>
                <p className='user-name'>Janica Queen</p>
                <p className='email'>janicaparadise@gmail.com</p>
            </div>
            <div className='user--dropdown'  onClick={() => setIsVisible(!isVisible)}>
                <span ><img src={chevronDown} alt='down arrow to open dropdown' className='arrow-down'/></span>
                <div className={'dropdown'}
                style={isVisible ? {display : 'block'} : {display : 'none'}}>
                    <div className='col-user' style={{
                        marginTop : 16,
                    }}>
                        <span><img src={userIcon} alt="user icon" /></span><p>User Profile</p>
                    </div>
                    <div className='col-user'>
                    <span><img src={userIcon} alt="user icon" /></span><p>Term & Services</p>
                    </div>
                    <div className='hr__line'></div>
                    <div className='log-out'>
                    <span><img src={warning} alt="user icon" /></span><p>Log Out</p>
                    </div>
                </div>
            </div>
        </div>
        <div className='rightSideBar-calender'>
            <Calendar className={"calender-right"} tileClassName={"calender_right-cell"}
            // showNavigation={false}
            />
        </div>

        <div className='right-instructors'>
                <p className='instructor__heading'>Instructors</p>
                <div className='instructor__detail'>
                    <span style={{
                        paddingLeft: 10
                    }} >
                        <img src={userIcon} className='intructor__pic'/>
                    </span>
                    <div className='instructor__text'>
                        <p className='instructor_name'>John Levis</p>
                        <p className='instructor_role'>EnglishTeacher</p>
                    </div>
                    <div className='icon__div'>
                        <span className='instructor__icon'>
                        <img  className = {"icon__info"}src={info} />

                        </span>
                        <span className='instructor__icon'>
                        <img className='icon__info' src={chatInfo} />

                        </span>
                    </div>
                </div>
                <div className='instructor__detail'>
                    <span style={{
                        paddingLeft: 10
                    }} >
                        <img src={userIcon} className='intructor__pic'/>
                    </span>
                    <div className='instructor__text'>
                        <p className='instructor_name'>John Levis</p>
                        <p className='instructor_role'>EnglishTeacher</p>
                    </div>
                    <div className='icon__div'>
                        <span className='instructor__icon'>
                        <img  className = {"icon__info"}src={info} />

                        </span>
                        <span className='instructor__icon'>
                        <img className='icon__info' src={chatInfo} />

                        </span>
                    </div>
                </div>
                <div className='instructor__detail'>
                    <span style={{
                        paddingLeft: 10
                    }} >
                        <img src={userIcon} className='intructor__pic'/>
                    </span>
                    <div className='instructor__text'>
                        <p className='instructor_name'>John Levis</p>
                        <p className='instructor_role'>EnglishTeacher</p>
                    </div>
                    <div className='icon__div'>
                        <span className='instructor__icon'>
                        <img  className = {"icon__info"}src={info} />

                        </span>
                        <span className='instructor__icon'>
                        <img className='icon__info' src={chatInfo} />

                        </span>
                    </div>
                </div>
        </div>
    </div>
  )
}

export default CalenderRight;
// navigationLabel={() => <p>Choose Date</p>}
