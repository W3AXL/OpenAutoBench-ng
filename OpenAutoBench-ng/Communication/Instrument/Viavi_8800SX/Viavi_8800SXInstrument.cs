﻿using OpenAutoBench_ng.Communication.Instrument.Connection;
using OpenAutoBench_ng.Communication.Instrument.IFR_2975;

namespace OpenAutoBench_ng.Communication.Instrument.Viavi_8800SX
{
    public class Viavi_8800SXInstrument: IBaseInstrument
    {
        private IInstrumentConnection Connection;

        public bool Connected { get; private set; }

        //TODO: get features and see if we are licensed for P25 or DMR
        public bool SupportsP25 { get; set; }

        public bool SupportsDMR { get; set; }

        public string Manufacturer { get; private set; }
        public string Model { get; private set; }
        public string Serial { get; private set; }
        public string Version { get; private set; }

        public int ConfigureDelay { get { return 250; } }

        public Viavi_8800SXInstrument(IInstrumentConnection conn)
        {
            Connected = false;
            Connection = conn;
            Connection.SetDelimeter("");
            SupportsP25 = true;
            SupportsDMR = true;
        }

        private async Task<string> Send(string command)
        {
            return await Connection.Send(command);
            //return await Connection.Send("\r\n");
        }

        private async Task Transmit(string command)
        {
            await Connection.Transmit(command);
        }

        public async Task Connect()
        {
            Connection.Connect();

        }

        public async Task Disconnect()
        {
            Connection.Disconnect();
        }

        public async Task<bool> TestConnection()
        {
            // TODO: Implement this
            Console.WriteLine("Connection test not yet implemented for instrument!");
            return false;
        }

        public async Task GenerateSignal(float power)
        {
            await Send($":gen:lvl:dbm {power}");
        }

        public async Task GenerateFMSignal(float power)
        {
            await Send($":gen:lvl:dbm {power}");
        }

        public async Task StopGenerating()
        {
            await Send($":gen:lvl:dbm -137");
        }

        public async Task SetGenPort(InstrumentOutputPort outputPort)
        {
            throw new NotImplementedException();
        }

        public async Task SetRxFrequency(int frequency, testMode mode)
        {
            await Transmit($":rec:freq {frequency / 1000000D}");
        }

        public async Task SetTxFrequency(int frequency)
        {
            await Transmit($":gen:freq {frequency / 1000000D}");
        }

        public async Task<float> MeasurePower()
        {
            return float.Parse(await Send(":rfpow:reading:avg?"));
        }

        public async Task<float> MeasureFrequencyError()
        {
            // returns in khz
            return float.Parse(await Send(":rferr:reading:val?")) * 1000;
        }

        public async Task<float> MeasureFMDeviation()
        {
            return float.Parse(await Send(":devmod:reading:val?"));
        }

        public async Task<bool> GetInfo()
        {
            Manufacturer = await Send(":options:man?");
            Model = await Send(":options:model?");
            Serial = await Send(":options:serial?");
            return true;
        }

        public async Task Reset()
        {
            await Send("*RST");
        }

        public async Task SetDisplay(InstrumentScreen screen)
        {
            //await Transmit("DISP " + displayName);
        }

        public async Task<float> MeasureP25RxBer()
        {
            throw new NotImplementedException();
            //string resp = await Send("Ber READING");
            // reading is percentage as decimal
            //return float.Parse(resp.Split(" ")[0]) * 100;
        }

        public Task<float> MeasureDMRRxBer()
        {
            throw new NotImplementedException();
        }

        public async Task ResetBERErrors()
        {
            throw new NotImplementedException();
            //await Send("Ber RESETERRors");
        }

        public async Task SetupRefOscillatorTest_P25()
        {
            //Not implemented, but shouldn't raise an exception
        }

        public async Task SetupRefOscillatorTest_FM()
        {
            //Not implemented, but shouldn't raise an exception
        }

        public async Task SetupTXPowerTest()
        {
            //Not implemented, but shouldn't raise an exception
        }

        public async Task SetupTXDeviationTest()
        {
            //Not implemented, but shouldn't raise an exception
        }

        public async Task SetupTXP25BERTest()
        {
            throw new NotImplementedException();
        }

        public async Task SetupRXTestFMMod()
        {
            //Not implemented, but shouldn't raise an exception
        }

        public async Task SetupRXTestP25BER()
        {
            //Not implemented, but shouldn't raise an exception

        }

        public async Task GenerateP25STDCal(float power)
        {
            //Not implemented, but shouldn't raise an exception
        }
    }
}
