export default interface SessionResponse {
    token: string;
    user: {
        id: number;
        roles: string[];
    };
}
