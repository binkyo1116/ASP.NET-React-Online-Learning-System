import React, { Component } from 'react'
import './Dashboard.scss'
import chevronDown from '../../assets/icons/chevron-down.svg'
import profileImg from '../../assets/images/profile-img.png'
import listImg1 from '../../assets/images/LWTech.png'
import listImg2 from '../../assets/icons/course-list.svg'
import chevronRight from '../../assets/icons/Chevron-right.svg'
import singleMessage from '../../assets/icons/Single-chat.svg'
import notification from '../../assets/icons/notification.svg'
import serchIcon from '../../assets/icons/Search.svg'
import user1 from '../../assets/images/user-1.png'
import user2 from '../../assets/images/user-2.png'
import user3 from '../../assets/images/user-3.png'
import user4 from '../../assets/images/user-4.png'
import { ProgressBar, Popover, OverlayTrigger,ListGroup,ListGroupItem } from 'react-bootstrap'
import Chart from './Chart'

const initialState = {

  
}
export default class index extends Component {
  constructor(props) {
    super(props)
    this.state = initialState;
    // this.imageRef = createRef();
  }
  popoverClickRootClose =() => {
    return (<Popover id="popover-trigger-click-root-close" >
      <ListGroup>
      <ListGroupItem href="#link1"title="Edit Profile">Edit Profile</ListGroupItem>
      <ListGroupItem title="Log Out" onClick={()=>{window.localStorage.clear();
        this.props.history.go('/');
      }} >Log Out</ListGroupItem>
    </ListGroup>
    </Popover>);
  }
  render() {
    return (
      <>
        <div className='col-6 main-container'>
          <div className='main-header'>
            <div>
              <h1>Welcome Back, Janice</h1>
              <div className='header-info'>
                <span>June 29, 2022</span>
                <label className='dot'></label>
                <span>6:32 AM</span>
              </div>
            </div>
            <div className='d-flex align-items-center'>
              <button className='btn-outline-square'>
                <img src={singleMessage} alt='' />
              </button>
              <button className='btn-outline-notification ml-3'>
                <img src={notification} alt='' />
              </button>
            </div>
          </div>
          <div className='secondary-header'>
            <label>My Course</label>
            <div className='input-with-icon'>
              <img className='input-icon' src={serchIcon} alt='' />
              <input placeholder='Search..' />
            </div>
          </div>
          <div className='row grid-card-container mb-3'>
            <div className='col-6'>
              <div className='card-container'>
                <div className='video-container'>
                  <div><label>Data 310</label></div>
                  <div>
                    <div className='video-part'>
                      <video controls >
                        <source src="https://interactive-examples.mdn.mozilla.net/media/cc0-videos/flower.webm" />
                      </video>
                    </div>
                    <div className='d-flex align-items-center justify-content-between'>
                      <label className='grade'>Grade A:</label>
                      {/* <div className='user-list'>
                        <img className='user-img-1' src={user1} alt='' />
                        <img className='user-img-2' src={user2} alt='' />
                        <img className='user-img-3' src={user3} alt='' />
                        <img className='user-img-4' src={user4} alt='' />
                        <div className='user-count'>
                          <label>16+</label>
                        </div>
                      </div> */}
                      <button className='btn-fill w-100px'>Enter Class</button>
                    </div>
                  </div>
                </div>
                <hr />
                <div className='d-flex align-items-center justify-content-center card-footer-part'>
                  {/* <label>0%</label> */}
                  <div className='w-75 custom-progress-bar'>
                    <ProgressBar className='progress-bar' now={62} />
                  </div>
                  <label>62%</label>
                </div>
              </div>
            </div>
            <div className='col-6'>
              <div className='card-container'>
                <div className='video-container'>
                  <div><label>CSD 233 - C++ Programming</label></div>
                  <div>
                    <div className='video-part'>
                      <video controls >
                        <source src="https://interactive-examples.mdn.mozilla.net/media/cc0-videos/flower.webm" />
                      </video>
                    </div>
                    <div className='d-flex align-items-center justify-content-between'>
                      <label className='grade'>Grade A:</label>
                      {/* <div className='user-list'>
                        <img className='user-img-1' src={user1} alt='' />
                        <img className='user-img-2' src={user2} alt='' />
                        <img className='user-img-3' src={user3} alt='' />
                        <img className='user-img-4' src={user4} alt='' />
                        <div className='user-count'>
                          <label>16+</label>
                        </div>
                      </div> */}
                      <button className='btn-fill w-100px'>Enter Class</button>
                    </div>
                  </div>
                </div>
                <hr />
                <div className='d-flex align-items-center justify-content-center card-footer-part'>
                  {/* <label>0%</label> */}
                  <div className='w-75 custom-progress-bar'>
                    <ProgressBar className='progress-bar' now={50} />
                  </div>
                  <label>50%</label>
                </div>
              </div>
            </div>
            <button className='btn-glassMorphism'>
              <img src={chevronRight} alt='' />
            </button>
          </div>
          <div className='graph-card-container'>
            <div className='graph-card-header'>
              <label>Grade Breakdown</label>
              <select name="Resource" id="resource">
                <option value="volvo">Data 310</option>
                <option value="saab">CSD 233 - C++</option>
                <option value="mercedes">Python</option>
              </select>
            </div>
            <div className='graph-card-body'>
              <Chart />
            </div>
          </div>
        </div>
        <div className='col-4 secondary-container'>
          <div className='secondary-card-header'>
            <label>Profile</label>
            <div>
              <button className='secondary-fill'>Edit</button>
              <OverlayTrigger
                trigger="click"
                rootClose
                placement="bottom"
                overlay={this.popoverClickRootClose}
              >
                <button className='blue-fill' ><img src={chevronDown} alt='' /></button>
              </OverlayTrigger>

            </div>
          </div>
          <div className='profile-container'>
            <div className='profile-img-container'>
              <img src={profileImg} alt='' />
            </div>
            <div className='profile-info'>
              <h2>Marcelo Guerra</h2>
              <label>janiceparadise@gmail.com</label>
            </div>
            <div className='row profile-list'>
              <div className='col-6'>
                <div className='profile-list-section'>
                  <div className='yellow-color'>
                    <img src={listImg1} alt='' />
                  </div>
                  <div className='profile-list-info'>
                    <label>School</label>
                    <span>LWTech</span>
                  </div>
                </div>
              </div>
              <div className='col-6'>
                <div className='profile-list-section'>
                  <div className='purple-color'>
                    <img src={listImg2} alt='' />
                  </div>
                  <div className='profile-list-info'>
                    <label>Courses</label>
                    <span>8 Courses</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className='upcoming-container'>
            <div className='upcoming-header'>
              <label>Upcoming</label>
              <button className='blue-btn-text'>See All <img src={chevronRight} alt='' /></button>
            </div>
            <hr />
            <div className='upcoming-card-body'>
              <div className='upcoming-list-container'>
                <div className='first-container'>
                  <label>9:00 AM</label>
                  <span>Fri, July 2nd</span>
                </div>
                <div className='middle-container'>
                  <label className='border-first'>&nbsp;</label>
                </div>
                <div className='last-container'>
                  <label>2nd Semester Test</label>
                  <span>Javascript Basic</span>
                </div>
              </div>
              <div className='upcoming-list-container'>
                <div className='first-container'>
                  <label>9:00 AM</label>
                  <span>Fri, July 2nd</span>
                </div>
                <div className='middle-container'>
                  <label className='border-second'>&nbsp;</label>
                </div>
                <div className='last-container'>
                  <label>2nd Semester Test</label>
                  <span>Javascript Basic</span>
                </div>
              </div>
              <div className='upcoming-list-container'>
                <div className='first-container'>
                  <label>9:00 AM</label>
                  <span>Fri, July 2nd</span>
                </div>
                <div className='middle-container'>
                  <label className='border-third'>&nbsp;</label>
                </div>
                <div className='last-container'>
                  <label>2nd Semester Test</label>
                  <span>Javascript Basic</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </>
    )
  }
}
