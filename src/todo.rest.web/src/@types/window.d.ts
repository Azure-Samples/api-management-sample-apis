export { };

declare global {
    interface Window {
        ENV_CONFIG: {
            TODO_REACT_REST_API_BASE_URL: string;
            TODO_REACT_REST_APPLICATIONINSIGHTS_CONNECTION_STRING: string;
        }
    }
}