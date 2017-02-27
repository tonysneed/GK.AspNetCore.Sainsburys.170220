import { Component } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'nav-menu',
    template: require('./navmenu.component.html'),
    styles: [require('./navmenu.component.css')]
})
export class NavMenuComponent {

    // Call username action to display the logged in user's name
    public loggedInAs: string
    public logoutUrl: string = '/account/logout';

    constructor(http: Http) {
        http.get('/account/username').subscribe(result => {
            let username = result.json();
            if(username.length > 0)
            this.loggedInAs = `Logged in as ${username}`;
        });
    }
}
