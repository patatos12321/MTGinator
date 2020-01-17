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
    public results: Result[];
    public loading: boolean;
    public eventId: number;
    public roundInProgress: boolean;
    public eventInProgress: boolean;

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private router: Router, route: ActivatedRoute) {
        this.SetBlankData();

        if (router.getCurrentNavigation().extras.state != null) {
            this.eventId = router.getCurrentNavigation().extras.state.id;
        }

        this.loading = true;
        this.eventInProgress = true;
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

        this.results = [];
    }

    SetPairingWinner(pairing: Pairing, player: Player) {
        pairing.winningPlayer = player;
    }

    RefreshEvent() {
        this.http.get<Event>(this.baseUrl + 'api/events/' + this.eventId).subscribe(result => {
            this.event = result;

            this.event.rounds.forEach(round => {
                round.pairings.forEach(pairing => {
                    pairing.players.forEach(player => {
                        if (player.id == pairing.winningPlayer.id) {
                            this.SetPairingWinner(pairing, player);
                        }
                    })
                })
            });

            this.NextRound();

        }, error => console.error(error));
    }

    NextRound() {
        this.http.get<Round>(this.baseUrl + 'api/events/' + this.eventId + '/next-round').subscribe(result => {
            if (this.event.rounds == null) {
                this.event.rounds = [];
            }

            this.event.rounds.push(result);
            this.loading = false;
            this.roundInProgress = true;
        }, error => console.error(error));
    }

    SubmitRound() {
        this.http.post(this.baseUrl + "api/events/" + this.eventId + "/round", this.event.rounds[this.event.rounds.length - 1]).subscribe(result => {
            this.roundInProgress = false;
        }, error => console.error(error));
    }

    GetResults() {
        this.http.get<Result[]>(this.baseUrl + "api/events/" + this.eventId + "/results").subscribe(result => {
            this.results = result;
            this.eventInProgress = false;
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

interface Round {
    id: number;
    number: number;
    pairings: Pairing[];
}

interface Pairing {
    id: number;
    players: Player[];
    winningPlayer: Player;
}

interface Player {
    name: string;
    id: number;
}

interface Result {
    nbWin: number;
    place: number;
    score: number;
    player: Player[];
}

