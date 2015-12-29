module app.account {
    'use strict';

    export class VerificationPhonenoRequest {
        PhoneNumber: string;
    }

    export class VerificationCodeRequest {
        PhoneNumber: string;
        VerificationCode: string;
    }

    export class VerificationCodeRespond {
        IsSuccess: boolean;
    }
    
}