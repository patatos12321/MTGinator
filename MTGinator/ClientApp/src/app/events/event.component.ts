import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'event',
    templateUrl: './event.component.html'
})


export class EventComponent {

    public event: Event;
    public error: string;
    public loading: boolean;
    public eventId: number;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router, route: ActivatedRoute) {
        this.SetBlankData();

        if (router.getCurrentNavigation().extras.state != null) {
          this.eventId = router.getCurrentNavigation().extras.state.id;
        } 

        if (this.eventId > 0) {
            this.loading = true;
            this.RefreshData();
        }
    }

    SetBlankData() {
        this.event = {
            name: null,
            imagePath: null,
            id: 0,
            date: null,
            official: true,
            participatingPlayers: []
        };
    }

    RefreshData() {
        this.http.get<Event>(this.baseUrl + 'api/events/' + this.eventId).subscribe(result => {
            this.event = result;
            this.loading = false;
        }, error => console.error(error));
    }

    Delete() {
        this.loading = true;

        this.http.delete(this.baseUrl + "api/events/" + this.eventId).subscribe(result => {
            this.router.navigate(['events']);
        }, error => console.error(error));
    }

    Save() {
        this.loading = true;

        this.http.post(this.baseUrl + "api/events", this.event).subscribe(result => {
            this.router.navigate(['/events']);
        }, error => console.error(error));
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
