class Creature {
  constructor(data) {
    this.id = data.id;
    this.name = data.name;
    this.type = data.type;
    this.stats = data.stats;
    this.rarity = data.rarity;
  }
}

module.exports = Creature;
