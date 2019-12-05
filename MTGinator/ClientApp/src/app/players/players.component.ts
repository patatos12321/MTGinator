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
    public error: string;
    public loading: boolean;

    private _httpClient: HttpClient;
    private _baseUrl: string;

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._httpClient = http;
        this._baseUrl = baseUrl;
        this.loading = true;

        this.ResetNewPlayer();
        this.RefreshData();
    }

    RefreshData() {
        this._httpClient.get<Player[]>(this._baseUrl + 'api/players').subscribe(result => {
            this.players = result;
            this.loading = false;
        }, error => console.error(error));
    }

    ResetNewPlayer() {
        this.newPlayer = {
            name: null,
            score: 0,
            id: 0,
            IsInEditMode: false
        };
    }

    DeletePlayer(id: number) {
        this.loading = true;

        this._httpClient.delete(this._baseUrl + "api/players/" + id).subscribe(result => {
            this.RefreshData();
        }, error => console.error(error));
    }

    EditPlayer(player: Player) {
        this.loading = true;

        this._httpClient.put(this._baseUrl + "api/players/" + player.id, player).subscribe(result => {
            this.RefreshData();
        }, error => console.error(error));
    }

    AddNewPlayer() {
        if (this.newPlayer.name == null || this.newPlayer.name == "") {
            this.error = "The player name can't be empty";
            return;
        }

        this.error = null;

        this.loading = true;
        this.players.push(this.newPlayer);

        this._httpClient.post(this._baseUrl + "api/players", this.players).subscribe(result => {
            this.RefreshData();
        }, error => console.error(error));

        this.ResetNewPlayer();
    }
}

interface Player {
    name: string;
    score: number;
    id: number;
    IsInEditMode: boolean;
}
