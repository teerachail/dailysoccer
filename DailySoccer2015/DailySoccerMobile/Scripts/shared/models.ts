module app.shared {
    'use strict';

    export class UserProfile {
        public UserId: string;
        public VerifiedPhoneNumber: string;
        public IsVerifiedFacebook: boolean;
    }

    export class TeamRequest {
        constructor(public id: string) { }
    }

}