import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'players',
    templateUrl: './players.component.html'
})


export class PlayersComponent {

    public players: Player[];

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {

      http.get<Player[]>(baseUrl + 'players').subscribe(result => {
          this.players = result;
      }, error => console.error(error));

  }
}

interface Player {
  name: string;
  score: number;
}
