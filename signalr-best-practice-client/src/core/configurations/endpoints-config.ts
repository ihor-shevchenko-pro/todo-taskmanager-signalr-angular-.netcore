// Base WebApi URL 
export const baseApiUrl = 'https://localhost:5001/api';
export const baseApiUrlShort = 'https://localhost:5001';

// SignalR
export const signalRHubEndpoints = {
    hubEndpoint: baseApiUrlShort + "/signalr",
}

// Account
export const accountEndpoints = {
    login: baseApiUrl + "/account/authorization_email",
    registration: baseApiUrl + "/account/registration_email",
};

// User
export const userEndpoints = {
    getCurrentUser: baseApiUrl + "/user/get_current_user",
    getUserById: baseApiUrl + "/user/get",
    getUsers: baseApiUrl + "/user/get",
};

//ToDoTask
export const toDoTaskEndpoints = {
    addToDoTaskSingleUser: baseApiUrl + "/todotask/add",
    addToDoTaskAllUsers: baseApiUrl + "/todotask/add_for_all",
    getSentTodotask: baseApiUrl + "/todotask/sent_todotasks",
    getReceivedTodotask: baseApiUrl + "/todotask/received_todotasks",
    changeTaskProgressStatus: baseApiUrl + "/todotask/progress_status",
}