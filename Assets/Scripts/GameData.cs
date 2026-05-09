[System.Serializable]

public struct GameData {
    public int puntos;
    public float tiempo;
    public string nombre;

    public GameData(int p, float t, string n) {
        this.puntos = p;
        this.tiempo = t;
        this.nombre = n;
    }

}
