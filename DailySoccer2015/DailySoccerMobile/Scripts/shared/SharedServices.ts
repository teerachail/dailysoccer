module app.shared {
    'use strict';

    interface IUserProfileService {
        GetUserProfile(): UserProfile;
        Logout(): void;
        LoggedInWithGuest(userId: string): void;
        LoggedInWithFacebook(userId: string, verifiedPhoneNo: string): void;
        UpdateVerifiedPhoneNo(phoneNo: string): void;
        DeleteVerifiedPhoneNo(): void;
    }

    export class UserProfileService implements IUserProfileService {

        private userprofile: UserProfile;

        constructor() {
            // HACK: Initial UserProfile (Load data from ionic user)
            this.userprofile = new UserProfile();
            this.userprofile.UserId = "u01guest";
            this.userprofile.IsVerifiedFacebook = true;
        }

        public GetUserProfile(): UserProfile {
            return this.userprofile;
        }
        public Logout(): void {
            this.userprofile.UserId = null;
            this.userprofile.IsVerifiedFacebook = false;
            this.userprofile.VerifiedPhoneNumber = null;
            this.updateUserProfile();
        }
        public LoggedInWithGuest(userId: string): void {
            this.userprofile.UserId = userId;
            this.userprofile.IsVerifiedFacebook = false;
            this.userprofile.VerifiedPhoneNumber = null;
            this.updateUserProfile();
        }
        public LoggedInWithFacebook(userId: string, verifiedPhoneNo: string): void {
            this.userprofile.UserId = userId;
            this.userprofile.IsVerifiedFacebook = true;
            this.userprofile.VerifiedPhoneNumber = verifiedPhoneNo;
            this.updateUserProfile();
        }
        public UpdateVerifiedPhoneNo(phoneNo: string): void {
            this.userprofile.VerifiedPhoneNumber = phoneNo;
            this.updateUserProfile();
        }
        public DeleteVerifiedPhoneNo(): void {
            this.userprofile.VerifiedPhoneNumber = null;
            this.updateUserProfile();
        }

        private updateUserProfile(): void {
            // HACK: UpdateUserProfile (Save data to ionic user)
        }
    }

    angular
        .module('app.shared')
        .service('app.shared.UserProfileService', UserProfileService);
}