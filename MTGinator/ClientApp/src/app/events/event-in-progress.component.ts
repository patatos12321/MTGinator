import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'event',
    templateUrl: './event-in-progress.component.html'
})


export class EventInProgressComponent {

    public event: Event;
    public loading: boolean;
    public eventId: number;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router, route: ActivatedRoute) {
        this.SetBlankData();

        if (router.getCurrentNavigation().extras.state != null) {
          this.eventId = router.getCurrentNavigation().extras.state.id;
        } 

        this.loading = true;
        this.RefreshEvent();
    }

    SetBlankData() {
        this.event = {
            name: null,
            imagePath: null,
            id: 0,
            date: null,
            official: true,
            participatingPlayers: [],
            rounds: []
        };
    }

    RefreshEvent() {
        this.http.get<Event>(this.baseUrl + 'api/events/' + this.eventId).subscribe(result => {
            this.event = result;

            this.http.get<Round>(this.baseUrl + 'api/events/' + this.eventId + '/next-round').subscribe(result => {
                if (this.event.rounds == null) {
                    this.event.rounds = [];
                }

                this.event.rounds.push(result);
                this.loading = false;
            }, error => console.error(error));

        }, error => console.error(error));
    }
}

interface Event {
    name: string;
    imagePath: number;
    id: number;
    date: Date;
    official: boolean;
    rounds: Round[];
    participatingPlayers: Player[];
}

interface Round
{
    id: number;
    number: number;
    pairings: Pairing[];
}

interface Pairing {
    id: number;
    players: Player[];
    winningPlayer: Player[];
}

interface Player {
    name: string;
    id: number;
}

interface PlayerModel {
    name: string;
    id: number;
    winning: boolean;
}

