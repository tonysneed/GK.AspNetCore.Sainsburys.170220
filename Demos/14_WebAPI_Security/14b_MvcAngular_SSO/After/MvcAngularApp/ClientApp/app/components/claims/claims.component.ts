import { Component } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'claims',
    template: require('./claims.component.html')
})
export class ClaimsComponent {

    public username: string
    public claims: Claim[];

    constructor(http: Http) {
        http.get('/username').subscribe(result => {
            this.username = result.json();
        });
        http.get('/claims').subscribe(result => {
            this.claims = result.json();
        });
    }
}

interface Claim {
    type: string;
    value: string;
}
