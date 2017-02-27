import { Component } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';

@Component({
    selector: 'fetchdata',
    template: require('./fetchdata.component.html')
})
export class FetchDataComponent {
    public forecasts: WeatherForecast[];

    constructor(http: Http) {
        // Get access token
        http.get('/account/accesstoken').subscribe(result => {
            if (result.status === 200) {
                let token = result.json();
                // Set authorization header using access token
                let headers = new Headers();
                headers.append('authorization', `Bearer ${token}`);
                let options = new RequestOptions({ headers: headers });
                // Use access token to invoke web api
                http.get('/api/SampleData/WeatherForecasts', options).subscribe(result => {
                    this.forecasts = result.json();
                });
            }
        });
    }
}

interface WeatherForecast {
    dateFormatted: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}
