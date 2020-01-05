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
    public possiblePlayers: PlayerModel[];
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
            this.RefreshEvent();
        }

        this.RefreshPlayers();
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

    RefreshPlayers() {
        this.http.get<Player[]>(this.baseUrl + 'api/players').subscribe(result => {
            this.possiblePlayers = [];

            result.forEach(player => {
                var isSelected = this.event.participatingPlayers.find(p => p.id == player.id) != null;

                this.possiblePlayers.push({
                    name: player.name,
                    id: player.id,
                    selected: isSelected
                });
            })

        }, error => console.error(error));
    }

    RefreshEvent() {
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

        this.event.participatingPlayers.length = 0;
        this.possiblePlayers.forEach(player => {
            if (player.selected) {
                this.event.participatingPlayers.push({
                    id: player.id,
                    name: player.name
                });
            }
        })

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
}

interface PlayerModel {
    name: string;
    id: number;
    selected: boolean;
}
