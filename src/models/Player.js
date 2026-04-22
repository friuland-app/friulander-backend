class Player {
  constructor(data) {
    this.id = data.id;
    this.name = data.name;
    this.level = data.level || 1;
    this.xp = data.xp || 0;
    this.inventory = data.inventory || [];
    this.creatures = data.creatures || [];
  }
}

module.exports = Player;
