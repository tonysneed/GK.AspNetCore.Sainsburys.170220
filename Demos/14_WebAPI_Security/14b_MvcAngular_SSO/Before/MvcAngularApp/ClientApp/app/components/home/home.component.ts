import { Component } from '@angular/core';

@Component({
    selector: 'home',
    template: require('./home.component.html')
})
export class HomeComponent {
    title: string
    constructor() {
        this.title = 'Hello Angular with OpenID Connect Implicit Flow!';
    }
}
