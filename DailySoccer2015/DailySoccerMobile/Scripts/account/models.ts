module app.account {
    'use strict';

    export class VerifyPhoneRequest {

        constructor(
            public id: string,
            public PhoneNumber: string,
            public VerificationCode: string = null) {
        }

    }

    export class SetFavoriteTeamRequest {
        constructor(public id: string, public TeamId: string) { }
    }

    export class VerifyCodeRespond {
        IsSuccess: boolean;
    }
    
}