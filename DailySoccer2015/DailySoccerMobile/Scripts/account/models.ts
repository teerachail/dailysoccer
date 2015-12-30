module app.account {
    'use strict';

    export class VerifyPhoneRequest {

        constructor(
            public id: string,
            public PhoneNumber: string,
            public VerificationCode: string = null) {
        }

    }

    export class VerifyCodeRespond {
        IsSuccess: boolean;
    }
    
}