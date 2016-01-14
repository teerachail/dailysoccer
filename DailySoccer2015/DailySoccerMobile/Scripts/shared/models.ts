module app.shared {
    'use strict';

    export class UserProfile {
        public UserId: string;
        public IsLoggedIn: boolean;
        public IsLoggedFacebook: boolean;
    }

    export class TeamRequest {
        constructor(public id: string) { }
    }

}