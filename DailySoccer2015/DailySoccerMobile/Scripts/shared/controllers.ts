module app.shared {
    'use strict';

    class SideMenuController {

        public Pages: number;

        static $inject = ['couponSummary', 'team', 'league', '$scope', '$ionicModal'];
        constructor(public couponSummary, public team, public league, private $scope, private $ionicModal) {
            this.$ionicModal.fromTemplateUrl('templates/Favorite.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(function (modal): void { $scope.Favorite = modal; });

            this.Pages = 0;

                this.$ionicModal.fromTemplateUrl('templates/Login.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(function (modal): void { $scope.Login = modal; });
        }
        
        public NextPages(): void {
            if (this.Pages < this.league.length - 1)
                this.Pages++;
            else
                this.Pages = 0;
        }

        public PreviousPages(): void {
            if (this.Pages > 0)
                this.Pages--;
            else
                this.Pages = this.league.length - 1;
        }

        public ShowPopUp(): void {
            this.$scope.Favorite.show();
        }

        public ClosePopUp(): void {
            this.$scope.Favorite.hide();
        }
        public LoginShow(): void {
            this.$scope.Login.show();
        }

        public LoginHide(): void {
            this.$scope.Login.hide();
        }
    }

    angular
        .module('app.shared')
        .controller('app.shared.SideMenuController', SideMenuController);

}