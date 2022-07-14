import * as React from 'react'
import './calender.scss'
import searchIcon from '../../assets/icons/Search.svg';
import chatInfo from '../../assets/icons/Single-chat.svg'
import notificationSvg from '../../assets/icons/notification.svg';
import TableCell from '@mui/material/TableCell';
import { ViewState, EditingState, IntegratedEditing } from '@devexpress/dx-react-scheduler';
import moment from 'moment';
import calender from '../../assets/icons/calender.svg';
import courseList from '../../assets/icons/course-list.svg'
import {
  Scheduler,
  Appointments,
  WeekView,
  AppointmentTooltip
} from '@devexpress/dx-react-scheduler-material-ui';
import Button from '@mui/material/Button';
const schedulerData = [
  {
    title: 'Video Editing',
    startDate: '2022-07-11T09:00',
    endDate: '2022-07-11T11:00',
    color: "#E2EAF5",
    textColor: '#2F80ED',
    sample: '3/12',
    border: '#2F80ED',
    credit: 5,
    testType: "Accessibility Test",
  },
  {
    title: 'Go to a gym',
    startDate: '2022-07-07T09:00',
    endDate: '2022-07-07T13:30',
    color: "#D6EADE",
    textColor: '#27AE60',
    sample: '3/12',
    border: '#27AE60',
    credit: 2,
    testType: "Accessibility Test",
  },
];


const DayScaleEmpty = (props) => {
  return (
    <WeekView.DayScaleEmptyCell style={{
      backgroundColor: ' #F5F5F5',
      height: 84,
      // width: 120,
      border: '1px solid transparent'
    }} {...props}/>
  )
}

const TickLayout =(props) => {
  return (
    <WeekView.TimeScaleTickCell style={{display: 'none',}} {...props} />
  )
}


const weekLayout = (props) => {
  return (
    <WeekView.TimeTableLayout {...props} style={{
      borderRight: '1.5px solid #D9D9D9',
      borderBottom: '1px solid #D9D9D9',
      borderTop : 0

    }} />
  )
}

const dayScaleCell = ({ startDate, endDate, today }) => {
let formatDate = moment(startDate).format('D');
if(formatDate < 10) {
  formatDate = 0 + formatDate;
}
  return (
  <TableCell style={{
    width: 89,
    height: 84,
    marginLeft : 20,
    borderBottom : '0px solid'
  }}>
    <div style={{
      display: 'flex',
      flexDirection : "column",
      alignItems : 'center'
    }}>

    <div style={{
      fontSize: 16.5
    }}>
      {formatDate}
    </div>
    <span style={{
      fontSize: 11
    }}>
      {moment(startDate).format('dddd')}
    </span>
    </div>

  </TableCell>
)};






const Appointment = ({ children, style, data, ...restProps }) => {
  return <Appointments.Appointment
    {...restProps}
    data={data}
    style={{
      ...style,
      backgroundColor: data.color,
      position: 'relative',
      color: data.textColor,
      borderLeft : '3px solid '+data.textColor,
      borderTopRightRadius : 10,
      width: 99
    }}
  >
    <div className='appointment__container'>
      <div className=' appointment__sample'>
        {data.sample}
      </div>
      <div className='center' style={{
        width: '58%',
        paddingLeft: 10
      }} >
        {data.title}
      </div>
      <div className='appointment__credit'>
        {data.credit} Credit
      </div>
      <div className='appointment__credit'>{data.startDate.slice(11, 13)} - {data.endDate.slice(11, 13)} pm</div>
    </div>
  </Appointments.Appointment>
}
const Content = ({
  children, appointmentData, ...restProps
}) => {
  console.log(appointmentData)
  return (
    <div className='appointment_tooltip'>
      <header>
        <h4>{appointmentData.title}</h4>
        <p>{`(${appointmentData.testType})`}</p>
      </header>
      <main>
        <div style={{
          display: 'flex',
          flexWrap: 'wrap'
        }}>

          <div className='appoitment__tooltip-cell'>
            <div className='appoitment__tooltip-field'>
              <span><img src={calender} alt='date' /></span><p>Date</p>
            </div>
            <div className='appoitment__tooltip-fieldValue'>{moment(appointmentData.startDate).format('MM dd YYYY')}</div>
          </div>
          <div className='appoitment__tooltip-cell'>
            <div className='appoitment__tooltip-field'>
              <span><img src={courseList} alt='Assignment' /></span><p>Assignment</p>
            </div>
            <div className='appoitment__tooltip-fieldValue'>2 Assignment</div>
          </div>
          <div className='appoitment__tooltip-cell'>
            <div className='appoitment__tooltip-field'>
              <span><img src={calender} alt='date' /></span><p>Date</p>
            </div>
            <div className='appoitment__tooltip-fieldValue'>{moment(appointmentData.startDate).format('MM dd YYYY')}</div>
          </div>
          <div className='appoitment__tooltip-cell'>
            <div className='appoitment__tooltip-field'>
              <span><img src={courseList} alt='Assignment' /></span><p>Assignment</p>
            </div>
            <div className='appoitment__tooltip-fieldValue'>2 Assignment</div>
          </div>
          <div className='appoitment__tooltip-cell'>
            <div className='appoitment__tooltip-field'>
              <span><img src={courseList} alt='Assignment' /></span><p>Assignment</p>
            </div>
            <div className='appoitment__tooltip-fieldValue'>2 Assignment</div>
          </div>

        </div>
      </main>
      <footer style={{
        display : 'flex',
        justifyContent : "space-between",
        marginRight : 35,
        marginTop : 30
      }}>
          <button style={{
            width: 167
          }} className='btn-fill'>Join</button>
          <button style={{
            width: 167
          }} className='btn-fill'>Leave</button>
      </footer>
    </div>
  )
};


const TimeScaleLayout = (props) => {
  return (
    <WeekView.TimeScaleLayout {...props} style={{
      backgroundColor: ' #F5F5F5',
      borderTop: 0,
      overflow : 'hidden',
      height : 1010

    }}
    className='timeScaleLayout'
     />
  )
}
const TimeTableCell = (props) => {
  return (
    <WeekView.TimeTableCell style={{
      // height: 20,
      width: 89,
      height: 84,

    }} {...props} >

    </WeekView.TimeTableCell>
  )
}
let startTime = 7

const TimeScalLabel = ({time}) => {
  if(startTime < 10) {
    startTime = 0 + startTime;
  }
// console.log("🚀 ~ file: calender.js ~ line 225 ~ TimeScalLabel ~ time", time)
if(time == undefined) {
  return <div style={{
    fontSize : 16,
    width: 116,
    fontFamily : 'Poppins',
    height: 84,
    top: 0.5,
    display: 'flex',
    fontWeight: 500,
    alignItems : 'center',
    borderBottom: '1px solid #D9D9D9',
    position: 'relative',
    right : -10
  }} className="timeScaleCell" >{moment(`Mon 03-Jul-2017, ${startTime} AM`, 'ddd DD-MMM-YYYY, hh:mm A').format('hh:mm: A')}
  </div>
}

  return (
  <div style={{
    fontSize : 16,
    width: 116,
    fontFamily : 'Poppins',
    height: 84,
    top: 0.5,
    display: 'flex',
    fontWeight: 500,
    alignItems : 'center',
    borderBottom: '1px solid #D9D9D9',
    position: 'relative',
    right : -10

  }}>{moment(time, 'ddd DD-MMM-YYYY, hh:mm A').format('hh:mm: A')}</div>)
}

const TooltipLayout = (props) => {
  return <AppointmentTooltip.Layout style={{
    width: 427,
    height: 486
  }} {...props} />
}
const Header = (props) => {
  return <AppointmentTooltip.Header  {...props} className="tooltip__header" style={{padding : 0, height : 0,}} />
}

function CalenderMain() {
  return (
    <div className='calender__main'>
      <header className='calender__header'>
        <div className='calaender__form-search'>
          <img src={searchIcon} alt="search icon" className='calender__form-svg' />
          <input type={"text"} className={"calender__form-input"} placeholder={"Search.."} />
        </div>
        <div className='header__icons'>

          <div className='calender__message-vector'>
            <span className='calender__svg-img'>
              <img src={chatInfo} alt='message' style={{ fill: 'transparent', stroke: 'transparent' }} />
            </span>
          </div>
          <div className='calender__message-vector'>
            <span className='calender__svg-img'>
              <img src={notificationSvg} alt="notification icons" />
            </span>
          </div>
        </div>

      </header>
      <main className='main__event-calender'>

          <Scheduler data={schedulerData}
            height={550}
            firstDayOfWeek={1}
          >

            <ViewState />
            <EditingState />
            <IntegratedEditing />
            <WeekView startDayHour={startTime} endDayHour={19}
              cellDuration={60}
              dayScaleCellComponent={dayScaleCell}
              timeTableCellComponent = {TimeTableCell}
              timeScaleLabelComponent={TimeScalLabel}
              timeScaleLayoutComponent={TimeScaleLayout}
              dayScaleEmptyCellComponent={DayScaleEmpty}
              timeTableLayoutComponent={weekLayout}
              timeScaleTickCellComponent={TickLayout}

            />
            <Appointments appointmentComponent={Appointment}
            />
            <AppointmentTooltip
              contentComponent={Content}
              showCloseButton
              layoutComponent={TooltipLayout}
              headerComponent={Header}
            />
          </Scheduler>
      </main>
    </div>
  )
}

export default CalenderMain
