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
import { VictoryBar, VictoryChart, VictoryAxis, TextSize, Bar, VictoryLabel, VictoryTheme } from 'victory';
import { styled } from '@mui/system'

const data = [
  {quarter: 1, earnings: 100},
  {quarter: 2, earnings: 100},
  {quarter: 3, earnings: 82},
  {quarter: 4, earnings: 87},
  {quarter: 5, earnings: 87},
  {quarter: 6, earnings: 87},
  {quarter: 7, earnings: 87},
  {quarter: 8, earnings: 87},
  {quarter: 9, earnings: 87},
  {quarter: 10, earnings: 87},
];

const initialState = {
  data: {
    labels: [
      "January",
      "February",
      "March",
      "April",
      "May",
      "June",
      "July"
    ],
    datasets: [
      {
        label: "My First dataset",
        backgroundColor: "#0069b4b3",
        borderColor: "#0069b4",
        borderWidth: 1,
        hoverBackgroundColor: "#0069b447",
        hoverBorderColor: "#0069b4",
        data: [65, 59, 100, 81, 56, 55, 40]
      }
    ]
  },
};
let quaters = [];
let tickValues = [];

for (let i = 1; i < 11; i ++) {
  quaters.push("Week" + i);
  tickValues.push(i);
}

const StyledBar = styled(Bar)`
  fill: purple;
`
const StyledLabel = styled(VictoryLabel)`
  tspan {
    fill: magenta;
    font-family: Papyrus, fantasy;
  }
`

// const colors = [
//   "#252525",
//   "#525252",
//   "#737373",
//   "#969696",
//   "#bdbdbd",
//   "#d9d9d9",
//   "#f0f0f0"
// ];
// const charcoal = "#252525";
// const grey = "#969696";

// // Typography
// const sansSerif = "'Gill Sans', 'Seravek', 'Trebuchet MS', sans-serif";
// const letterSpacing = "normal";
// const fontSize = 14;

// // Layout
// const baseProps = {
//   width: 450,
//   height: 300,
//   padding: 50,
//   colorScale: colors
// };

// // Labels
// const baseLabelStyles = {
//   fontFamily: sansSerif,
//   fontSize,
//   letterSpacing,
//   padding: 10,
//   fill: charcoal,
//   stroke: "transparent"
// };

// const centeredLabelStyles = assign({ textAnchor: "middle" }, baseLabelStyles);

// // Strokes
// const strokeLinecap = "round";
// const strokeLinejoin = "round";

// // Put it all together...

// const theme = {
//   area: assign(
//     {
//       style: {
//         data: {
//           fill: charcoal
//         },
//         labels: baseLabelStyles
//       }
//     },
//     baseProps
//   ),
//   axis: assign(
//     {
//       style: {
//         axis: {
//           fill: "transparent",
//           stroke: charcoal,
//           strokeWidth: 1,
//           strokeLinecap,
//           strokeLinejoin
//         },
//         axisLabel: assign({}, centeredLabelStyles, {
//           padding: 25
//         }),
//         grid: {
//           fill: "none",
//           stroke: "none",
//           pointerEvents: "painted"
//         },
//         ticks: {
//           fill: "transparent",
//           size: 1,
//           stroke: "transparent"
//         },
//         tickLabels: baseLabelStyles
//       }
//     },
//     baseProps
//   ),
//   bar: assign(
//     {
//       style: {
//         data: {
//           fill: charcoal,
//           padding: 8,
//           strokeWidth: 0
//         },
//         labels: baseLabelStyles
//       }
//     },
//     baseProps
//   ),
//   boxplot: assign(
//     {
//       style: {
//         max: { padding: 8, stroke: charcoal, strokeWidth: 1 },
//         maxLabels: assign({}, baseLabelStyles, { padding: 3 }),
//         median: { padding: 8, stroke: charcoal, strokeWidth: 1 },
//         medianLabels: assign({}, baseLabelStyles, { padding: 3 }),
//         min: { padding: 8, stroke: charcoal, strokeWidth: 1 },
//         minLabels: assign({}, baseLabelStyles, { padding: 3 }),
//         q1: { padding: 8, fill: grey },
//         q1Labels: assign({}, baseLabelStyles, { padding: 3 }),
//         q3: { padding: 8, fill: grey },
//         q3Labels: assign({}, baseLabelStyles, { padding: 3 })
//       },
//       boxWidth: 20
//     },
//     baseProps
//   ),
//   candlestick: assign(
//     {
//       style: {
//         data: {
//           stroke: charcoal,
//           strokeWidth: 1
//         },
//         labels: assign({}, baseLabelStyles, { padding: 5 })
//       },
//       candleColors: {
//         positive: "#ffffff",
//         negative: charcoal
//       }
//     },
//     baseProps
//   ),
//   chart: baseProps,
//   errorbar: assign(
//     {
//       borderWidth: 8,
//       style: {
//         data: {
//           fill: "transparent",
//           stroke: charcoal,
//           strokeWidth: 2
//         },
//         labels: baseLabelStyles
//       }
//     },
//     baseProps
//   ),
//   group: assign(
//     {
//       colorScale: colors
//     },
//     baseProps
//   ),
//   histogram: assign(
//     {
//       style: {
//         data: {
//           fill: grey,
//           stroke: charcoal,
//           strokeWidth: 2
//         },
//         labels: baseLabelStyles
//       }
//     },
//     baseProps
//   ),
//   legend: {
//     colorScale: colors,
//     gutter: 10,
//     orientation: "vertical",
//     titleOrientation: "top",
//     style: {
//       data: {
//         type: "circle"
//       },
//       labels: baseLabelStyles,
//       title: assign({}, baseLabelStyles, { padding: 5 })
//     }
//   },
//   line: assign(
//     {
//       style: {
//         data: {
//           fill: "transparent",
//           stroke: charcoal,
//           strokeWidth: 2
//         },
//         labels: baseLabelStyles
//       }
//     },
//     baseProps
//   ),
//   pie: {
//     style: {
//       data: {
//         padding: 10,
//         stroke: "transparent",
//         strokeWidth: 1
//       },
//       labels: assign({}, baseLabelStyles, { padding: 20 })
//     },
//     colorScale: colors,
//     width: 400,
//     height: 400,
//     padding: 50
//   },
//   scatter: assign(
//     {
//       style: {
//         data: {
//           fill: charcoal,
//           stroke: "transparent",
//           strokeWidth: 0
//         },
//         labels: baseLabelStyles
//       }
//     },
//     baseProps
//   ),
//   stack: assign(
//     {
//       colorScale: colors
//     },
//     baseProps
//   ),
//   tooltip: {
//     style: assign({}, baseLabelStyles, { padding: 0, pointerEvents: "none" }),
//     flyoutStyle: {
//       stroke: charcoal,
//       strokeWidth: 1,
//       fill: "#f0f0f0",
//       pointerEvents: "none"
//     },
//     flyoutPadding: 5,
//     cornerRadius: 5,
//     pointerLength: 10
//   },
//   voronoi: assign(
//     {
//       style: {
//         data: {
//           fill: "transparent",
//           stroke: "transparent",
//           strokeWidth: 0
//         },
//         labels: assign({}, baseLabelStyles, { padding: 5, pointerEvents: "none" }),
//         flyout: {
//           stroke: charcoal,
//           strokeWidth: 1,
//           fill: "#f0f0f0",
//           pointerEvents: "none"
//         }
//       }
//     },
//     baseProps
//   )
// };

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
                      <div className='user-list'>
                        <img className='user-img-1' src={user1} alt='' />
                        <img className='user-img-2' src={user2} alt='' />
                        <img className='user-img-3' src={user3} alt='' />
                        <img className='user-img-4' src={user4} alt='' />
                        <div className='user-count'>
                          <label>16+</label>
                        </div>
                      </div>
                      <button className='btn-fill w-100px'>Enter Class</button>
                    </div>
                  </div>
                </div>
                <hr />
                <div className='d-flex align-items-center justify-content-center card-footer-part'>
                  <label>0%</label>
                  <div className='w-75 custom-progress-bar'>
                    <ProgressBar now={60} />
                  </div>
                  <label>C</label>
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
                      <div className='user-list'>
                        <img className='user-img-1' src={user1} alt='' />
                        <img className='user-img-2' src={user2} alt='' />
                        <img className='user-img-3' src={user3} alt='' />
                        <img className='user-img-4' src={user4} alt='' />
                        <div className='user-count'>
                          <label>16+</label>
                        </div>
                      </div>
                      <button className='btn-fill w-100px'>Enter Class</button>
                    </div>
                  </div>
                </div>
                <hr />
                <div className='d-flex align-items-center justify-content-center card-footer-part'>
                  <label>0%</label>
                  <div className='w-75 custom-progress-bar'>
                    <ProgressBar now={10} />
                  </div>
                  <label>C</label>
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
            <VictoryChart
              // domainPadding will add space to each side of VictoryBar to
              // prevent it from overlapping the axis
              domainPadding={10}
              height={221}
              width={550}
              theme={VictoryTheme.material}
            >
              {/* <VictoryAxis dependentAxis
                domain={[-10, 15]}
                offsetX={50}
                orientation="left"
                standalone={false}
                
              /> */}
              <VictoryAxis
                // tickValues specifies both the number of ticks and where
                // they are placed on the axis
                tickValues={tickValues}
                tickFormat={quaters}
                style={{ }}
              />
              <VictoryAxis
                dependentAxis
                // tickFormat specifies how ticks should be displayed
                tickValues={[0, 25, 50, 75, 100]}
                tickFormat={(x) => x}
              />
              <VictoryBar
                style={{ data: { fill: "#AADBEF" } }}
                cornerRadius={{ top: 4, bottom: 4 }}
                data={data}
                width={33}
                x="quarter"
                y="earnings"
              />
            </VictoryChart>
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
