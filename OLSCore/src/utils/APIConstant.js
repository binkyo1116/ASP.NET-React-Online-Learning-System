//=============PATH=================//
import { apiBase } from './config';
const SERVER_PATH = apiBase;
export default class APIConstant {
  static path = {

    // Auth API
    login: `${SERVER_PATH}/login`,


   //Course
    Course: `${SERVER_PATH}/Course`
  };
}
