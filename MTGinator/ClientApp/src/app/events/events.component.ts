import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'events',
    templateUrl: './events.component.html'
})


export class EventsComponent {

    public events: Event[];
    public error: string;
    public loading: boolean;

    private _httpClient: HttpClient;
    private _baseUrl: string;

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._httpClient = http;
        this._baseUrl = baseUrl;
        this.loading = true;

        this.RefreshData();
    }

    RefreshData() {
        this._httpClient.get<Event[]>(this._baseUrl + 'api/events').subscribe(result => {
            this.events = result;
            this.loading = false;
        }, error => console.error(error));
    }

    DeleteEvent(id: number) {
        this.loading = true;

        this._httpClient.delete(this._baseUrl + "api/events/" + id).subscribe(result => {
            this.RefreshData();
        }, error => console.error(error));
    }
}

interface Event {
    name: string;
    imagePath: number;
    id: number;
    date: Date;
    official: boolean;
}

interface Player {
    name: string;
    id: number;
}
