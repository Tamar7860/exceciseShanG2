import axios from 'axios';
import  Item from './ItemModel.ts';
const apiUrl = "http://localhost:5274";
axios.defaults.baseURL = apiUrl;

// Add an interceptor to handle errors in the response
axios.interceptors.response.use(
  (response) => response,
  (error) => {
    // Log the error to the console or send it to your logging service
    console.error('API Error:', error);

    // Pass the error along to the calling code
    return Promise.reject(error);
  }
);
export default {
  getTasks: async () => {
    const result = await axios.get(`${apiUrl}/items`)    
    return result.data;
  },

  addTask: async(newItem)=>{
    try {
      console.log('addTask');
      const result = await axios.post(`${apiUrl}/items`, newItem);
      return result.data; // You might return the result if needed
    } catch (error) {
      console.error('Error in addTask:', error);
      throw error; // Handle or propagate the error as needed
    }
  },

  setCompleted: async(id, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    //TODO
    return {};
  },

  deleteTask:async(id)=>{
    try {
      const result = await axios.delete(`${apiUrl}/items/${id}`);
      return result.data; 
    } catch (error) {
      console.error('Error in deleteTask:', error);
      throw error; 
    }
  }
};
