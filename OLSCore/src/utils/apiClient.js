import axios from "axios";
import { apiBase } from "../utils/config";

const instance = axios.create({
  baseURL: apiBase,
  timeout: 60000,
  responseType: "json",
});

var headers = {
  "Access-Control-Allow-Origin": "*",
  "Access-Control-Allow-Methods": "*",
  "Access-Control-Allow-Headers": "*",
  "Content-Type": "application/json; charset=UTF-8",
};

const request = (method, url, data) => {
  return new Promise((resolve, reject) => {
    (() => {
      if (method === "get") {
        return instance.request({
          url,
          method,
          params: data,
          headers: headers,
        });
      } else {
        return instance.request({
          url,
          method,
          data,
          headers: headers,
        });
      }
    })()
      .then((res) => {
        resolve(res.data);
      })
      .catch((err) => {
        reject(err.response);
      });
  });
};

// eslint-disable-next-line import/no-anonymous-default-export
export default {
  get: (endpoint, data) => {
    return request("get", endpoint, data);
  },
  post: (endpoint, data) => {
    return request("post", endpoint, data);
  },
  put: (endpoint, data) => {
    return request("put", endpoint, data);
  },
  del: (endpoint, data) => {
    return request("delete", endpoint, data);
  },
};
