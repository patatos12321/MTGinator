import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'players',
    templateUrl: './players.component.html'
})


export class PlayersComponent {

    public players: Player[];
    public newPlayer: Player;
    private _httpClient: HttpClient;
    private _baseUrl: string;
    private _loading: boolean;

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._httpClient = http;
        this._baseUrl = baseUrl;
        this._loading = true;

        this.RefreshData();

        this.ResetNewPlayer();
    }

    RefreshData() {
        this._httpClient.get<Player[]>(this._baseUrl + 'players').subscribe(result => {
            this.players = result;
            this._loading = false;
        }, error => console.error(error));
    }

    ResetNewPlayer() {
        this.newPlayer = {
            name: null,
            score: 0
        };
    }

    AddNewPlayer() {
        this._loading = true;
        this.players.push(this.newPlayer);

        this._httpClient.post(this._baseUrl + "players", this.players).subscribe(result => {
            this.RefreshData();
        }, error => console.error(error));

        this.ResetNewPlayer();
    }
}

interface Player {
    name: string;
    score: number;
}
