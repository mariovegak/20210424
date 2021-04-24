using Microsoft.ML.Data;

namespace Detecciondellamadasanomalas
{
    // Datos de llamadas extraidos del archivo CSV
    public class Llamada
    {
        [LoadColumn(0)]
        public string fecha;

        [LoadColumn(1)]
        public double tiempo;
    }

    // Donde se guarda las predicciones de llamadas
    public class PrediccionDeLlamada
    {
        [VectorType(7)]
        public double[] Prediccion { get; set; }
    }

}
