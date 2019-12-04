import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
    selector: 'new-player',
    templateUrl: './new-player.component.html'
})


export class NewPlayerComponent {

    public newPlayer: Player;
    private _baseUrl: string;
    private _httpClient: HttpClient;

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this._httpClient = http;
        this._baseUrl = baseUrl;
        this.newPlayer = {
            name: null,
            score: 0
        };
    }

    AddNewPlayer() {
        this._httpClient.post(this._baseUrl + "player", this.newPlayer);
    }
}

interface Player {
  name: string;
  score: number;
}
