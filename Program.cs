using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.TimeSeries;
using System.Collections;
using System.Collections.Generic;


namespace Detecciondellamadasanomalas
{
    class Program
    {


        // funcion que detecta las anomalias
        static void DetectAnomaly(MLContext mlContext, IDataView Llamadas, int period, Boolean imprimir, string textFileName)
        {
            Console.WriteLine("Detectando las anomalías");
            // Inicializando los valores limites.
            var options = new SrCnnEntireAnomalyDetectorOptions()
            {
                Threshold = 0.3, // Valor considerado como anormal o umbral
                Sensitivity = 64.0, // Determina el rango del limite
                DetectMode = SrCnnDetectMode.AnomalyAndMargin, // Tipo de Anomalía a detectar
                Period = period, // Periodo o frencuencia que se presentan los datos
            };
            // Datos de Salida luego de ser procesados los datos y verificados
            IDataView outputDataView =
                mlContext
                    .AnomalyDetection.DetectEntireAnomalyBySrCnn( // Detecta una anomalia usando SRCNN
                        Llamadas,
                        nameof(PrediccionDeLlamada.Prediccion),
                        nameof(Llamada.tiempo),
                        options);
            // Crea la lista de Peedicciones
            IEnumerable<PrediccionDeLlamada> predictions = mlContext.Data.CreateEnumerable<PrediccionDeLlamada>(
                outputDataView, reuseRowObject: false);
            if (imprimir)
            {
                using (var stream = File.Create(textFileName))
                        mlContext.Data.SaveAsText(outputDataView, stream,';');
            }
            // Encabezado
            Console.WriteLine("Indice\t Anomalia\t Valor_esperado\t\t Limite_Superior\t Limite_inferior");
            var index = 0;
            foreach (var p in predictions)
            {
                if (p.Prediccion[0] == 1)
                {
                    Console.WriteLine("{0}\t {1}\t\t {2}\t {3}\t {4}  <-- alerta detectada un anomalia", index,
                        p.Prediccion[0], p.Prediccion[3], p.Prediccion[5], p.Prediccion[6]);
                }
                else
                {
                    Console.WriteLine("{0}\t {1}\t\t {2}\t {3}\t {4}", index,
                        p.Prediccion[0], p.Prediccion[3], p.Prediccion[5], p.Prediccion[6]);
                }
                ++index;
            }
            Console.WriteLine("");
        }
        // Función que determina el período de la serie usando Furier
        static int DetectPeriod(MLContext mlContext, IDataView Llamadas)
        {
            Console.WriteLine("Determina el período de la serie");
            // Se le pasa el el Contexto y el valor de la Vista de llamadas retorna el periodo
            // Por el método de Furier detecta la frecuencia, sin no existe retorna -1 ( tomar en cuenta )
            int period = mlContext.AnomalyDetection.DetectSeasonality(Llamadas, nameof(Llamada.tiempo));
            Console.WriteLine("El period para la serie es: {0}.", period);
            return period;
        }

        // CUERPO PRINCIPAL DEL PROGRAMA
        // ARCHIVO CON LAS LLAMADAS
        // Puede asignarle el path directo _dataPath=@"C:\Data\llamadas-telefonicas-anomalias.csv";
        // o guardarlo en el directorio de ejecucion en el Debug \obj\Debug\netcoreapp3.1\Data
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "llamadas-telefonicas-anomalias.csv");
        static readonly string _dataPathOUT = Path.Combine(Environment.CurrentDirectory, "Data", "Prediccion-anomalias.csv");
        static void Main(string[] args)
        {
            Console.WriteLine("Detección de llamadas anómalas");
            // Crea el objeto Seed for MLContext's random number generator
            MLContext mlContext = new MLContext();
            // Genera el DataView ( formato de tabla o matriz ) a partir de un acrhivo plano
            IDataView dataView = mlContext.Data.LoadFromTextFile<Llamada>(path: _dataPath, hasHeader: true, separatorChar: ',');
            //Determinar la serie en la muestra por Fourier, en caso de -1 es que no encontró el periodo
            int period = DetectPeriod(mlContext, dataView);
            // Detecta la anomalía en la muesta con el período definido.
            DetectAnomaly(mlContext, dataView, period,true, _dataPathOUT);
            Console.WriteLine("Fin!");
        }
    }
}
