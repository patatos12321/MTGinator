import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
    selector: 'events',
    templateUrl: './events.component.html'
})


export class EventsComponent {

    public events: Event[];
    public error: string;
    public loading: boolean;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router) {
        this.loading = true;

        this.RefreshData();
    }

    RefreshData() {
        this.http.get<Event[]>(this.baseUrl + 'api/events').subscribe(result => {
            this.events = result;
            this.loading = false;
        }, error => console.error(error));
    }

    EditEvent(id: number) {
        this.loading = true;
        this.router.navigate(['/event/'], { state: { id: id } });
    }

    StartEvent(id: number) {
        this.loading = true;
        this.router.navigate(['/event-in-progress/'], { state: { id: id } });
    }
}

interface Event {
    name: string;
    imagePath: number;
    id: number;
    date: Date;
    official: boolean;
    participatingPlayers: Player[];
}

interface Player {
    name: string;
    id: number;
    score: number;
}
