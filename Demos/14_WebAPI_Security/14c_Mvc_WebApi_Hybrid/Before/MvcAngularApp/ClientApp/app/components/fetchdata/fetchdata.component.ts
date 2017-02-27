import { Component } from '@angular/core';
// TODO: Add Headers and RequestOptions imports from http
import { Http } from '@angular/http';
// import { Http, Headers, RequestOptions } from '@angular/http';

@Component({
    selector: 'fetchdata',
    template: require('./fetchdata.component.html')
})
export class FetchDataComponent {
    public forecasts: WeatherForecast[];

    constructor(http: Http) {
        // TODO: Refactor to retrieve access token and use for request
        http.get('/api/SampleData/WeatherForecasts').subscribe(result => {
            this.forecasts = result.json();
        });
        // http.get('/account/accesstoken').subscribe(result => {
        //     if (result.status === 200) {
        //         let token = result.json();
        //         let headers = new Headers();
        //         headers.append('authorization', `Bearer ${token}`);
        //         let options = new RequestOptions({ headers: headers });
        //         http.get('/api/SampleData/WeatherForecasts', options).subscribe(result => {
        //             this.forecasts = result.json();
        //         });
        //     }
        // });
    }
}

interface WeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
